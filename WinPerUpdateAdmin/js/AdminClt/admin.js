(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'serviceAdmin'];

    function admin($scope, serviceAdmin) {
        $scope.title = 'admin';

        activate();

        function activate() {
            $scope.versiones = [];
            $scope.totales = [0, 0];
            $scope.idUsuario = $("#idToken").val();

            serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                //console.log(JSON.stringify(cliente));
                serviceAdmin.getVersiones(cliente.Id).success(function (data) {
                    angular.forEach(data, function (version) {
                        $scope.versiones.push(version);
                        if (version.Estado == 'P' || version.Estado == 'C') $scope.totales[0]++;
                        else if (version.Estado == 'N') $scope.totales[1]++;
                    });
                }).
                error(function (data) {
                    console.error(data);
                });
            })
            .error(function (err) {
                console.error(err);
            });

        }
    }
})();
