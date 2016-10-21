(function () {
    'use strict';

    angular
        .module('app')
        .controller('mantenedor', mantenedor);

    mantenedor.$inject = ['$scope','$window', '$routeParams', 'serviceAmbientes'];

    function mantenedor($scope,$window, $routeParams, serviceAmbientes) {
        $scope.title = 'Mantenedor Ambiente';

        activate();

        function activate() {

            $scope.idAmbiente = (angular.isUndefined($routeParams.idAmbiente) ? 0 : $routeParams.idAmbiente);
            $scope.idCliente = $routeParams.idCliente;
            $scope.labelcreate = ($scope.idAmbiente == 0) ? "Crear" : "Modificar";
            $scope.increate = true;
            $scope.tipos = [{ "Codigo": 1, "Descripcion": "Producción" }, { "Codigo": 2, "Descripcion": "Desarrollo" }]

            if ($scope.idAmbiente > 0) {
                serviceAmbientes.getAmbiente($scope.idCliente, $scope.idAmbiente).success(function (data) {
                    console.log(JSON.stringify(data));
                    $scope.ambiente = data;
                }).error(function (data) {
                    console.error(data);
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

                        console.log(JSON.stringify(data));
                        $scope.idAmbiente = data.idAmbientes;
                    }).error(function (data) {
                        console.error(data);
                    });
                } else {//Se Modifica
                    serviceAmbientes.updAmbiente($scope.idCliente, $scope.idAmbiente, formData.Nombre, formData.Tipo, formData.ServerBd, formData.Instancia, formData.NomBd, formData.UserDbo, formData.PwdDbo).success(function (data) {
                        $scope.increate = true;
                        $scope.labelcreate = "Modificar";
                    }).error(function (data) {
                        console.error(data);
                    });
                }
            }

            $scope.Eliminar = function () {
                serviceAmbientes.delAmbiente($scope.idCliente, $scope.idAmbiente).success(function () {
                    $window.setTimeout(function () {
                        $window.location.href = "/AmbientesClt#/";
                    }, 2000);
                }).error(function (data) {
                    console.error(data);
                });
            }

        }
    }
})();
