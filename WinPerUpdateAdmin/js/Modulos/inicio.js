(function () {
    'use strict';

    angular
        .module('app')
        .controller('inicio', inicio);

    inicio.$inject = ['$scope', 'serviceModulos', 'FileUploader', '$window'];

    function inicio($scope, serviceModulos, FileUploader, $window) {
        $scope.title = 'Módulos';
        $scope.idUsuario = $("#idToken").val();
        $scope.modulos = [];
        $scope.modulosxlsx = [];
        $scope.modxlsxwarn = false;
        $scope.sync = 0;
        $scope.msgSuccess = "";
        $scope.logSync = "";
        activate();

        function activate() {
            serviceModulos.listarModulos().success(function (data) {
                $scope.modulos = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });
            $scope.CargarModXLSX = function () {
                $("#modxlsx-modal").modal('show');
                serviceModulos.getModulosXlsx($scope.idUsuario).success(function (data) {
                    $scope.modulosxlsx = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                $scope.msgSuccess = "";
                if (response.CodErr == 0) {
                    $scope.uploader.clearQueue();
                    $('#export-modal').modal('toggle');
                    if (response.sModulo == "OK") {
                        serviceModulos.listarModulos().success(function (data) {
                            $scope.modulos = data;
                            $scope.modxlsxwarn = false;
                            $scope.msgError = "";
                            $scope.msgSuccess = "Módulos importados y agregados correctamente.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    } else {
                        $scope.modxlsxwarn = true;
                    }
                } else {
                    console.error(response.MsgErr); $scope.msgError = response.MsgErr; window.scrollTo(0, 0);
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
