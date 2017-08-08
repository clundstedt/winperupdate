
(function () {
    'use strict';

    angular
        .module('app')
        .controller('controlcambios', controlcambios);

    controlcambios.$inject = ['$scope', '$window', '$routeParams', 'serviceAdmin', '$timeout', 'FileUploader'];

    function controlcambios($scope, $window,$routeParams, serviceAdmin, $timeout, FileUploader) {
        $scope.title = 'titulo';

        activate();

        function activate() {
            window.scrollTo(0, 0);
            $scope.idVersion = 0;
            $scope.msgError = "";
            $scope.msgSuccess = "";
            $scope.formData = {};
            $scope.version = null;
            $scope.modulos = [];
            $scope.chkimpacta = false;
            
            $scope.DocCambiosEliminar = "";
            $scope.controlCambios = null;

            $scope.nameFiles = [];

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idVersion = $routeParams.idVersion;

                if (!angular.isUndefined($routeParams.tips) && !angular.isUndefined($routeParams.modulo)) {
                    serviceAdmin.getControlCambiosEx($scope.idVersion, $routeParams.tips, $routeParams.modulo).success(function (data) {
                        $scope.controlCambios = data;
                        $scope.formData.modulo = data.Modulo;
                        $scope.formData.release = data.Release;
                        $scope.formData.tips = data.Tips;
                        $scope.formData.desccc = data.Descripcion;
                        if (data.Impacto == "No impacta otras funcionalidades") $scope.chkimpacta = true;
                        $scope.formData.impacto = data.Impacto;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); 
                    });
                }

                serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                    $scope.version = data;
                    serviceAdmin.getModulosVersion($scope.idVersion).success(function (modulosVersion) {
                        $scope.msgError = "";
                        $scope.modulos = modulosVersion;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            }

            $scope.ShowMdlDeleteDocCambios = function (file) {
                $scope.DocCambiosEliminar = file;
                $("#mdlDeleteDocCambios").modal('show');
            }

            $scope.ShowMdlDeleteControlCambios = function () {
                $("#mdlDeleteControlCambios").modal('show');
            }

            $scope.EliminarDocCambios = function (file) {
                serviceAdmin.delDocCambios(file, $scope.idVersion, $scope.controlCambios.Tips, $scope.controlCambios.Modulo).success(function (data) {
                    $scope.controlCambios = data;
                    $scope.msgSuccess = "Documento eliminado correctamente!.";
                    window.scrollTo(0, 0);
                    $scope.msgError = "";
                    $("#mdlDeleteDocCambios").modal('toggle');
                }).error(function (err) {
                    $("#mdlDeleteDocCambios").modal('toggle');
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            }

            $scope.ModificarControlCambios = function () {
                if ($scope.uploader.queue.length > 0) {
                    $scope.uploader.uploadAll();
                } else {
                    serviceAdmin.updControlCambios($scope.idVersion, $scope.formData.modulo, $scope.formData.release, $scope.formData.tips, $scope.formData.desccc, $scope.nameFiles, $scope.formData.impacto).success(function (data) {
                        $scope.controlCambios = data;
                        $scope.formData.modulo = data.Modulo;
                        $scope.formData.release = data.Release;
                        $scope.formData.tips = data.Tips;
                        $scope.formData.desccc = data.Descripcion;
                        $scope.formData.impacto = data.Impacto;
                        $scope.msgSuccess = "Control de cambios modificado correctamente!.";
                        $scope.nameFiles = [];
                        window.scrollTo(0, 0);
                        $scope.uploader.clearQueue();
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                    });
                }
            }

            $scope.EliminarControlCambios = function () {
                serviceAdmin.delControlCambios($scope.idVersion, $scope.controlCambios.Tips, $scope.controlCambios.Modulo).success(function (data) {
                    $("#mdlDeleteControlCambios").modal('toggle');
                    $timeout(function () {
                        $window.location.href = "/Admin/#/EditVersion/" + $scope.idVersion;
                    }, 2000);
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            }

            var uploader = $scope.uploader = new FileUploader();


            // FILTERS

            // a sync filter
            uploader.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
                }
            });

            // an async filter
            uploader.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            // CALLBACKS
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                $scope.nameFiles.push(fileItem.file.name);
                //console.info("onSuccessItem", fileItem, response, status, headers);
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

            uploader.onAfterAddingFile = function (fileItem) {
                $scope.msgError = "";
                var fiSplit = fileItem.file.name.split('.');
                if (fiSplit[fiSplit.length - 1] != "docx" && fiSplit[fiSplit.length - 1] != "doc") {
                    fileItem.remove();
                    $scope.msgError = "El archivo debe ser un documento Word.";
                    window.scrollTo(0, 0);
                }
                if (fileItem.file.name.length > 50) {
                    fileItem.remove();
                    $scope.msgError = "El nombre del archivo no puede contener más de 50 carácteres, incluyendo la extensión.";
                    window.scrollTo(0, 0);
                }
                $scope.nameFiles = [];
                fileItem.url = '/Admin/UploadCambios?idVersion=' + $scope.idVersion + '&idModulo=' + $scope.formData.modulo + '&tips=' + $scope.formData.tips;
            };

            uploader.onCompleteAll = function () {
                $scope.msgError = "";
                $scope.msgSuccess = "";
                if ($scope.controlCambios != null) {
                    serviceAdmin.updControlCambios($scope.idVersion, $scope.formData.modulo, $scope.formData.release, $scope.formData.tips, $scope.formData.desccc, $scope.nameFiles, $scope.formData.impacto).success(function (data) {
                        $scope.controlCambios = data;
                        $scope.formData.modulo = data.Modulo;
                        $scope.formData.release = data.Release;
                        $scope.formData.tips = data.Tips;
                        $scope.formData.desccc = data.Descripcion;
                        $scope.formData.impacto = data.Impacto;
                        $scope.msgSuccess = "Control de cambios modificado correctamente!.";
                        $scope.nameFiles = [];
                        window.scrollTo(0, 0);
                        $scope.uploader.clearQueue();
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                    });
                } else {
                    serviceAdmin.addControlCambios($scope.idVersion, $scope.formData.modulo, $scope.formData.release, $scope.formData.tips, $scope.formData.desccc, $scope.nameFiles, $scope.formData.impacto).success(function (data) {
                        $scope.msgSuccess = "Control de cambios agregado correctamente!.";
                        $scope.nameFiles = [];
                        window.scrollTo(0, 0);
                        $scope.formData.tips = "";
                        $scope.formData.release = "";
                        $scope.formData.desccc = "";
                        $scope.formData.impacto = "";
                        $scope.chkimpacta = false;
                        $scope.uploader.clearQueue();
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                    });
                }
            }
        }

    }
})();
