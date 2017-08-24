(function () {
    'use strict';

    angular
        .module('app')
        .controller('componente', componente);

    componente.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'FileUploader', '$window'];

    function componente($scope, $routeParams, serviceAdmin, FileUploader, $window) {
        $scope.title = 'componente';
        $scope.OkSegunExistencia = false;
        activate();

        function activate() {
            $scope.msgError = "";
            $scope.msgError2 = [];

            $scope.componentes = [];
            $scope.idVersion = $routeParams.idVersion;
            $scope.componentesOficiales = [];
            $scope.modulos = [];


            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            serviceAdmin.getComponentesOficiales().success(function (data) {
                $scope.componentesOficiales = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            $scope.VerificaComponentesOkSegunFecha = function () {
                var paso = false;
                for (var i = 0; i < $scope.componentesOficiales.length; i++) {
                    for (var j = 0; j < uploader.queue.length; j++) {
                        if ($scope.componentesOficiales[i].Name == uploader.queue[j].file.name) {
                            if (new Date($scope.componentesOficiales[i].LastWrite) > uploader.queue[j].file.lastModifiedDate) {
                                paso = true;
                            }
                        }
                    }
                }
                if (paso || VerificaComponentesOkSegunExistencia()) {
                    $("#errorcompsegfec-modal").modal('show');
                } else {
                    uploader.uploadAll();
                }
            }

            $scope.ComponenteOkSegunFecha = function (file) {
                for (var i = 0; i < $scope.componentesOficiales.length; i++) {
                    if ($scope.componentesOficiales[i].Name == file.name) {
                        if (new Date($scope.componentesOficiales[i].LastWrite) > file.lastModifiedDate) {
                            return false;
                        }
                    }
                }
                return true;
            }


            $scope.VerificaComponentesOkSegunExistencia = function () {
                var paso = false;
                for (var i = 0; i < uploader.queue.length; i++) {
                    if (uploader.queue[i].isError) {
                        paso = true;
                    }
                }
                if (paso) {
                    $("#errorcompsegfec-modal").modal('show');
                } else {
                    uploader.uploadAll();
                }
            }

            var uploadOptions = {
                url: '/Admin/Upload?idVersion=' + $scope.idVersion,
                filters: []
            };

            // Numero maximo de archivos en la cola
            uploadOptions.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
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
                    serviceAdmin
                        .addComponente($scope.idVersion, response.sModulo, fileItem.file.name, fileItem.file.lastModifiedDate.toISOString(), response.sVersion)
                        .success(function (data) {
                            $scope.msgError = "";
                        })
                        .error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
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
            uploader.onAfterAddingFile = function (fileItem) {
                serviceAdmin.existeComponente(fileItem.file.name).success(function (data) {
                    fileItem.isError = !data;
                    if (!fileItem.isError) {
                        serviceAdmin.isModuloVigente(fileItem.file.name).success(function (isVigente) {
                            if (isVigente) {
                                $scope.msgError = "";
                            } else {
                                $scope.msgError2.push(fileItem.file.name);
                                fileItem.remove();
                            }
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    } else {
                        $scope.msgError = "";
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    fileItem.isError = false;
                });
            };

            //console.info('uploader', uploader);
        }
    }
})();
