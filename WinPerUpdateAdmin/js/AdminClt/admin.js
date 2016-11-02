(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'serviceAmbientes'];

    function admin($scope, $routeParams, serviceAdmin, serviceAmbientes) {
        $scope.title = 'admin';

        activate();

        function activate() {
            $scope.version = {};
            $scope.versiones = [];
            $scope.ambientes = [];
            $scope.totales = [0, 0, 0];
            $scope.idUsuario = $("#idToken").val();
            $scope.idAmbiente = 0;
            $scope.mensaje = "";

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idversion = $routeParams.idVersion;
                $scope.titulo = "Modificar Versión";
                $scope.labelcreate = "Modificar";

                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.version = data;
                    $scope.version.Estado = 'N';

                    angular.forEach($scope.version.Componentes, function (item) {
                        if (item.Tipo == 'exe') $scope.totales[0]++;
                        else if (item.Tipo == 'qrp') $scope.totales[1]++;
                        else $scope.totales[2]++;
                    });

                    serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                        //console.log(JSON.stringify(cliente));
                        serviceAdmin.getAmbientes(cliente.Id, $scope.idversion).success(function (ambiente) {
                            $scope.ambientes = ambiente;
                        }).
                        error(function (err) {
                            console.error(err);
                        });
                    })
                    .error(function (err) {
                        console.error(err);
                    });

                }).error(function (err) {
                    console.error(err);
                });
            }
            else {
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

            $scope.ShowConfirmPublish = function (id, nombre) {
                $scope.nombreambiente = nombre;
                $scope.idAmbiente = id;
                $scope.estaVigente = false;
                $("#publish-modal").modal('show');
            }

            $scope.Publicar = function () {
                serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                    //console.log(JSON.stringify(cliente));
                    serviceAdmin.addVersion($scope.idversion, cliente.Id, $scope.idAmbiente, 'V').success(function () {
                        $scope.mensaje = "Versión agregada exitosamente";
                        angular.forEach($scope.ambientes, function (item) {
                            if (item.idAmbientes == $scope.idAmbiente) {
                                item.Estado = 'V';
                                $scope.estaVigente = true;
                            }
                        });
                    }).
                    error(function (err) {
                        console.error(err);
                    });
                })
                .error(function (err) {
                    console.error(err);
                });
            }
        }
    }
})();
