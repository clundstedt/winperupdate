(function () {
    'use strict';

    angular
        .module('app')
        .controller('inicio', inicio);

    inicio.$inject = ['$scope', 'serviceModulos', 'FileUploader'];

    function inicio($scope, serviceModulos,FileUploader) {
        $scope.title = 'Modulos';
        $scope.idUsuario = $("#idToken").val();
        $scope.modulos = [];
        $scope.modulosxlsx = [];
        $scope.modxlsxwarn = false;
        $scope.sync = 0;
        $scope.logSync = "";
        activate();

        function activate() {
            serviceModulos.listarModulos().success(function (data) {
                $scope.modulos = data;
            }).error(function (err) {
                console.error(err);
            });
            $scope.CargarModXLSX = function () {
                $("#modxlsx-modal").modal('show');
                serviceModulos.getModulosXlsx($scope.idUsuario).success(function (data) {
                    $scope.modulosxlsx = data;
                }).error(function (err) {
                    console.error(err);
                });
            }

            $scope.SyncComponentes = function () {
                if ($scope.sync == 0) {
                    $scope.sync = 1;
                    serviceModulos.syncComponentes().success(function (dataSync) {
                        //dataSync: contiene reporte de los componentes NO sincronizados
                        $scope.logSync = dataSync;
                        $scope.sync = 2;
                    }).error(function (errorSync) {
                        $scope.logSync = errorSync;
                        $scope.sync = 2;
                    });
                } else {
                    $scope.sync = 0;
                }
            }


            $scope.downloadFile = function () {
                window.location = '/api/Modulos/Planilla';
            }

            //Proceso de upload
            var uploadOptions = {
                url: '/Modulos/Upload?idUsuario=' + $scope.idUsuario,
                filters: []
            };

            uploadOptions.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 1;
                }
            });
            // File must not be larger then some size
            uploadOptions.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            $scope.uploadOptions = uploadOptions;

            var uploader = $scope.uploader = new FileUploader();

            // CALLBACKS
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                if (response.CodErr == 0) {
                    $scope.uploader.clearQueue();
                    $('#export-modal').modal('toggle');
                    if (response.sModulo == "OK") {
                        serviceModulos.listarModulos().success(function (data) {
                            $scope.modulos = data;
                            $scope.modxlsxwarn = false;
                            console.log($scope.modulos);
                        }).error(function (err) {
                            console.error(err);
                        });
                    } else {
                        $scope.modxlsxwarn = true;
                    }
                } else {
                    console.error(response.MsgErr);
                }

            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                //console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploader.onCancelItem = function (fileItem, response, status, headers) {
                //console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                //console.info('onCompleteItem', fileItem, response, status, headers);
            };
        }
    }
})();
