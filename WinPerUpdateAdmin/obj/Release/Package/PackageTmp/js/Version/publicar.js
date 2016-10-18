(function () {
    'use strict';

    angular
        .module('app')
        .controller('publicar', publicar);

    publicar.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'serviceClientes'];

    function publicar($scope, $routeParams, serviceAdmin, serviceClientes) {
        $scope.title = 'publicar';

        activate();

        function activate() {
            $scope.clientes = [];
            $scope.mensaje = "";
            $scope.totclientes = 0;

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idVersion = $routeParams.idVersion;

                serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                    $scope.version = data;
                }).error(function (data) {
                    console.error(data);
                });

                serviceClientes.getClientes().success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var item = {
                            Check: false,
                            Tipo: 0,
                            Cliente: data[i]
                        };
                        $scope.clientes.push(item);
                    }
                }).error(function (data) {
                    console.error(data);
                });
            }

            $scope.Agregar = function () {
                angular.forEach($scope.clientes, function (item) {
                    if (item.Tipo == 0 && item.Check) {
                        item.Tipo = 1;
                        item.Check = false;

                        $scope.totclientes++;
                    }
                });
            }

            $scope.Eliminar = function () {
                angular.forEach($scope.clientes, function (item) {
                    if (item.Tipo == 1 && item.Check) {
                        item.Tipo = 0;
                        item.Check = false;

                        $scope.totclientes--;
                    }
                });
            }

            $scope.ShowConfirm = function () {
                $("#publish-modal").modal('show');
            }

            $scope.Publicar = function () {
                var index = 0;
                angular.forEach($scope.clientes, function (item) {
                    if (item.Tipo == 1) {
                        console.log("Agregando Cliente " + item.Cliente.Id + " a la version " + $scope.idVersion);
                        serviceAdmin.addCliente($scope.idVersion, item.Cliente.Id).success(function (data) {
                            index++;
                            if (index == $scope.totclientes) {
                                $scope.mensaje = "Versión publicada exitosamente.";
                            }
                        }).error(function (data) {
                            console.error(data);
                        });
                    }
                });
            }
        }
    }
})();
