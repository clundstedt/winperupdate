(function () {
    'use strict';

    angular
        .module('app')
        .controller('inicio', inicio);

    inicio.$inject = ['$scope', 'serviceClientes'];

    function inicio($scope, serviceClientes) {
        $scope.title = 'inicio';

        activate();

        function activate() {
            $scope.msgError = "";

            $scope.clientes = [];
            $scope.all = false;

            serviceClientes.getClientes().success(function (data) {
                $scope.clientes = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            $scope.GenerarPDF = function () {
                window.location = '/api/Clientes/PDF';
            }

        }
    }
})();
