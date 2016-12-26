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
            $scope.clientes = [];
            $scope.all = false;

            serviceClientes.getClientes().success(function (data) {
                $scope.clientes = data;
            }).error(function (data) {
                console.error(data);
            });

            $scope.GenerarPDF = function () {
                window.location = '/api/Clientes/PDF';
            }

        }
    }
})();
