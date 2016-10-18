(function () {
    'use strict';

    angular
        .module('app')
        .controller('seguridad', seguridad);

    seguridad.$inject = ['$scope', 'serviceSeguridad']; 

    function seguridad($scope, serviceSeguridad) {
        $scope.title = 'seguridad';

        activate();

        function activate() {
            console.log("id User = " + $("#idToken").val());

            $scope.idCliente = 0;
            $scope.idUsuario = $("#idToken").val();
            $scope.usuarios = [];
            $scope.totales = [0, 0];

            serviceSeguridad.getUsuario($scope.idUsuario).success(function (data) {
                $scope.idCliente = data.Cliente.Id;

                serviceSeguridad.getUsuarios($scope.idCliente).success(function (usuarios) {
                    $scope.usuarios = usuarios;

                    angular.forEach($scope.usuarios, function (item) {
                        $scope.totales[item.CodPrf - 11]++;
                    });

                }).error(function (err) {
                    console.error(err);
                });

            }).error(function (err) {
                console.error(err);
            });
        }
    }
})();
