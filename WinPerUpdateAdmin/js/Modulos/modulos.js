(function () {
    'use strict';

    angular
        .module('app')
        .controller('modulos', modulos);

    modulos.$inject = ['$scope', '$routeParams','serviceModulos'];

    function modulos($scope, $routeParams, serviceModulos) {
        $scope.title = 'modulos';
        $scope.formData = {};
        $scope.idModulo = 0;
        $scope.usuario = $("#idToken").val();
        $scope.loading = false;

        $scope.tipoalert = "";
        $scope.msgalert = "";

        $scope.accion = "Crear";

        activate();

        function activate() {


            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idModulo = $routeParams.idModulo;
                serviceModulos.getModulo($scope.idModulo).success(function (data) {
                    $scope.formData.nombre = data.NomModulo;
                    $scope.formData.descripcion = data.Descripcion;
                    $scope.formData.iscore = data.isCore;
                    $scope.formData.directorio = data.Directorio;
                    $scope.formData.estado = data.Estado;
                    $scope.accion = "Modificar";
                }).error(function (err) {
                    console.error(err);
                });
            }

            $scope.Accion = function (formData) {
                if ($scope.accion == "Crear") {
                    $scope.Crear(formData);
                } else if ($scope.accion == "Modificar") {
                    $scope.Modificar(formData);
                }
            }

            $scope.Crear = function (formData) {
                $scope.loading = true;
                serviceModulos.addModulo(formData.nombre, formData.descripcion, formData.iscore, formData.directorio).success(function (data) {
                    $scope.accion = "Modificar";
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo creado exitosamente!.";
                    $scope.idModulo = data.idModulo;
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de creación del módulo, vuelva a intentarlo.";
                });
                $scope.loading = false;
            }

            $scope.Modificar = function (formData) {
                $scope.loading = true;
                serviceModulos.updModulo($scope.idModulo, formData.nombre, formData.descripcion, formData.iscore, formData.directorio).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo modificado exitosamente!."
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert="Ocurrió un error durante el proceso de modificación, vuelva a intentarlo."
                });
                $scope.loading = false;
            }

            $scope.Eliminar = function () {
                $scope.loading = true;
                serviceModulos.delModulo($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo fue caducado con exito!.";
                    $scope.formData.estado = 'C';
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de caducación del modulo, vuelva a intentarlo.";
                });
                $("#confelim-modal").modal('toggle');
                $scope.loading = false;
            }

            $scope.Vigente = function () {
                $scope.loading = true;
                serviceModulos.setVigente($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo establecido como vigente exitosamente!.";
                    $scope.formData.estado = 'V';
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso, vuelva a intentarlo.";
                });
                $("#confvigen-modal").modal('toggle');
                $scope.loading = false;
            }
        }
    }
})();
