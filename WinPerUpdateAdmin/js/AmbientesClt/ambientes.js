(function () {
    'use strict';

    angular
        .module('app')
        .controller('ambientes', ambientes);

    ambientes.$inject = ['$scope', 'serviceAmbientes', 'serviceSeguridad' ];

    function ambientes($scope, serviceAmbientes, serviceSeguridad) {
        $scope.title = 'Ambientes';

        activate();

        function activate() {
            $scope.ambientes = [];
            $scope.idCliente = 0;
            console.log("id User = " + $("#idToken").val());

            serviceSeguridad.getUsuario($("#idToken").val()).success(function (data) {
                $scope.idCliente = data.Cliente.Id;

                serviceAmbientes.getAmbientes($scope.idCliente).success(function (ambientes) {
                    console.log(JSON.stringify(ambientes));
                    $scope.ambientes = ambientes;
                }).error(function (err) {
                    console.error(err);
                });

            }).error(function (err) {
                console.error(err);
            });

        }
    }
})();
