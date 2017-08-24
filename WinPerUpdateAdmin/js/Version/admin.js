(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'serviceAdmin', '$window'];

    function admin($scope, serviceAdmin, $window) {
        $scope.msgError = "";
        $scope.formData = {};
        $scope.formData.hasdeploy = true;
        $scope.title = 'admin';
        $scope.lblLoad = "";
        $scope.btnLoadHabilitado = false;
        $scope.totales = [0, 0, 0];
        activate();

        function activate() {
            $scope.versiones = [];
            $scope.msgVersionInicial = "";
            
            serviceAdmin.getVersiones().success(function (data) {
                angular.forEach(data, function (version) {
                    $scope.versiones.push(version);
                    if (version.Estado == 'N') $scope.totales[0]++;
                    else if (version.Estado == 'P') $scope.totales[1]++;
                    else if (version.Estado == 'C') $scope.totales[2]++;
                });
                $scope.msgError = "";
            }).
            error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });
            
            $scope.GenerarVersionInicial = function (formData, valid) {
                if (!valid) {
                    $scope.msgVersionInicial = "Verifique los campos e intentelo nuevamente!.";
                } else {
                    $("#inicial-modal").modal('toggle');
                    $('#loading-modal').modal({ backdrop: 'static', keyboard: false })
                    serviceAdmin.getExisteRelease(formData.release).success(function (existeRel) {
                        if (!existeRel) {
                            $scope.lblLoad = "Creando versión inicial.";
                            $scope.btnLoadHabilitado = false;
                            serviceAdmin.addVersionInicial(formData.release, 'N', formData.descripcion, !formData.hasdeploy).success(function (data) {
                                $scope.lblLoad = "Agregando componentes de versión inicial.";
                                serviceAdmin.GenVersionInicial(data.IdVersion).success(function (data1) {
                                    $scope.lblLoad = "Obteniendo información de la version inicial.";
                                    serviceAdmin.getVersiones().success(function (data) {
                                        $scope.versiones = data;
                                        $scope.btnLoadHabilitado = true;
                                        $('#loading-modal').modal('toggle');
                                        $scope.msgError = "";
                                    }).error(function (err) {
                                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                        $scope.lblLoad = "Ocurrió un error al obtener la información de la versión inicial. Verifique la consola del navegador.";
                                        $scope.btnLoadHabilitado = true;
                                    });
                                }).error(function (err) {
                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                    $scope.lblLoad = "Ocurrió un error al agregar los componentes de la versión inicial. Verifique la consola del navegador.";
                                    $scope.btnLoadHabilitado = true;
                                });
                            }).error(function (err) {
                                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                $scope.lblLoad = "Ocurrió un error al crear la versión inicial. Verifique la consola del navegador.";
                                $scope.btnLoadHabilitado = true;
                            });
                        } else {
                            $scope.lblLoad = "El release de la versión ya existe, utilice otro e inténtelo nuevamente.";
                            $scope.btnLoadHabilitado = true;
                        }
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                        $scope.lblLoad = "Ocurrió un error al crear la versión inicial. Verifique la consola del navegador.";
                        $scope.btnLoadHabilitado = true;
                    });
                }
            }
        }
    }
})();
