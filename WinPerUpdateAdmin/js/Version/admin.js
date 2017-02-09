(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'serviceAdmin'];

    function admin($scope, serviceAdmin) {
        $scope.title = 'admin';
        $scope.lblLoad = "";
        $scope.btnLoadHabilitado = false;

        activate();

        function activate() {
            $scope.versiones = [];
           

            serviceAdmin.getVersiones().success(function (data) {
                $scope.versiones = data;
            }).
            error(function (data) {
                console.error(data);
            });

            $scope.GenerarVersionInicial = function (formData) {
                $('#loading-modal').modal({ backdrop: 'static', keyboard: false })
                $scope.lblLoad = "Creando versión inicial.";
                $scope.btnLoadHabilitado = false;
                serviceAdmin.addVersionInicial(formData.release, 'N', formData.descripcion).success(function (data) {
                    $scope.lblLoad = "Agregando componentes de versión inicial.";
                    serviceAdmin.GenVersionInicial(data.IdVersion).success(function (data1) {
                        $scope.lblLoad = "Obteniendo información de la version inicial.";
                        serviceAdmin.getVersiones().success(function (data) {
                            $scope.versiones = data;
                            $scope.btnLoadHabilitado = true;
                            $('#loading-modal').modal('toggle');
                        }).error(function (err) {
                            console.error(err);
                            $scope.lblLoad = "Ocurrió un error al obtener la información de la versión inicial. Verifique la consola del navegador.";
                            $scope.btnLoadHabilitado = true;
                        });
                    }).error(function (err) {
                        console.error(err);
                        $scope.lblLoad = "Ocurrió un error al agregar los componentes de la versión inicial. Verifique la consola del navegador.";
                        $scope.btnLoadHabilitado = true;
                    });
                }).error(function (err) {
                    console.error(err);
                    $scope.lblLoad = "Ocurrió un error al crear la versión inicial. Verifique la consola del navegador.";
                    $scope.btnLoadHabilitado = true;
                });
            }
        }
    }
})();
