(function () {
    'use strict';

    angular
        .module('app')
        .controller('publicar', publicar);

    publicar.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'serviceClientes', '$window'];

    function publicar($scope, $routeParams, serviceAdmin, serviceClientes, $window) {
        $scope.title = 'publicar';

        activate();

        function activate() {
            $scope.msgError = "";

            $scope.clientes = [];
            $scope.clientesVersion = [];
            $scope.mensaje = "";
            $scope.totclientes = 0;

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idVersion = $routeParams.idVersion;

                serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                    $scope.version = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

                serviceClientes.getClientes().success(function (data) {
                    serviceClientes.getClientesVersion($scope.idVersion).success(function (data2) {
                        for (var i = 0; i < data.length; i++) {
                            var item = {
                                Check: false,
                                Tipo: 0,
                                Lectura: true,
                                Cliente: data[i]
                            };
                            $scope.clientes.push(item);
                        }
                        for (var i = 0; i < data.length; i++) {
                            var idClt = $scope.clientes[i].Cliente.Id;
                            for (var j = 0; j < data2.length; j++) {
                                if (idClt == data2[j].Id) {
                                    $scope.clientes[i].Tipo = 1;
                                    $scope.clientes[i].Lectura = false;
                                }
                            }
                        }
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                    if (item.Tipo == 1 && item.Lectura) {
                        console.log("Agregando Cliente " + item.Cliente.Id + " a la version " + $scope.idVersion);
                        serviceAdmin.addCliente($scope.idVersion, item.Cliente.Id).success(function (data) {
                            index++;
                            if (index == $scope.totclientes) {
                                $scope.mensaje = "Versión publicada exitosamente.";
                            }
                            $scope.msgError = "";
                        }).error(function (err) {
                            $scope.mensaje = "Ocurrió un error durante el proceso de publicación, verifique la consola";
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    }
                });
            }
        }
    }
})();
