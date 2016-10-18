(function () {
    'use strict';

    angular
        .module('app')
        .controller('seguridad', seguridad);

    seguridad.$inject = ['$scope', '$routeParams', 'serviceSeguridad'];

    function seguridad($scope, $routeParams, serviceSeguridad) {
        $scope.title = 'seguridad';

        activate();

        function activate() {
            $scope.usuarios = [];
            $scope.totales = [0, 0, 0, 0, 0];
            $scope.perfiles = [{ CodPrf: 1, NomPrf: 'Administrador' },
                               { CodPrf: 2, NomPrf: 'Desarrollador' },
                               { CodPrf: 3, NomPrf: 'Soporte' },
                               { CodPrf: 4, NomPrf: 'Gestión' }];
            $scope.perfilactivo = 1;

            serviceSeguridad.getUsuarios().success(function (data) {
                $scope.usuarios = data;

                angular.forEach($scope.usuarios, function (item) {
                    $scope.totales[item.CodPrf]++;
                });

            }).error(function (err) {
                console.error(err);
            });

            $scope.Panel = function (idPanel) {
                console.log("panel activo = " + idPanel);
                $scope.perfilactivo = idPanel;
            }
        }
    }
})();
