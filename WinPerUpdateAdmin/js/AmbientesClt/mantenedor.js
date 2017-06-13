(function () {
    'use strict';

    angular
        .module('app')
        .controller('mantenedor', mantenedor);

    mantenedor.$inject = ['$scope', '$window', '$routeParams', 'serviceAmbientes'];

    function mantenedor($scope, $window, $routeParams, serviceAmbientes) {
        $scope.title = 'Mantenedor Ambiente';

        activate();

        function activate() {
            $scope.msgError = "";

            $scope.idAmbiente = (angular.isUndefined($routeParams.idAmbiente) ? 0 : $routeParams.idAmbiente);
            $scope.idCliente = $routeParams.idCliente;
            $scope.labelcreate = ($scope.idAmbiente == 0) ? "Crear" : "Modificar";
            $scope.increate = true;
            $scope.tipos = [{ "Codigo": 1, "Descripcion": "Producción" }, { "Codigo": 2, "Descripcion": "Pruebas" }]



            if ($scope.idAmbiente > 0) {
                serviceAmbientes.getAmbiente($scope.idCliente, $scope.idAmbiente).success(function (data) {
                    console.log(JSON.stringify(data));
                    $scope.ambiente = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.SaveAmbiente = function (formData) {
                console.log(JSON.stringify(formData));
                $scope.increate = false;
                $scope.labelcreate = "Enviando";
                //Se Crea
                if ($scope.idAmbiente == 0) {
                    serviceAmbientes.addAmbiente($scope.idCliente, formData.Nombre, formData.Tipo, formData.ServerBd, formData.Instancia, formData.NomBd, formData.UserDbo, formData.PwdDbo).success(function (data) {
                        $scope.increate = true;
                        $scope.labelcreate = "Modificar";
                        $scope.msgError = "";
                        console.log(JSON.stringify(data));
                        $scope.idAmbiente = data.idAmbientes;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                } else {//Se Modifica
                    serviceAmbientes.updAmbiente($scope.idCliente, $scope.idAmbiente, formData.Nombre, formData.Tipo, formData.ServerBd, formData.Instancia, formData.NomBd, formData.UserDbo, formData.PwdDbo).success(function (data) {
                        $scope.increate = true;
                        $scope.labelcreate = "Modificar";
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
            }

            $scope.Eliminar = function () {
                serviceAmbientes.delAmbiente($scope.idCliente, $scope.idAmbiente).success(function () {
                    $window.setTimeout(function () {
                        $window.location.href = "/AmbientesClt#/";
                    }, 2000);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

        }
    }
})();
