(function () {
    'use strict';

    angular
        .module('app')
        .controller('componente', componente);

    componente.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'FileUploader'];

    function componente($scope, $routeParams, serviceAdmin, FileUploader) {
        $scope.title = 'componente';
        $scope.OkSegunExistencia = false;
        activate();

        function activate() {
            $scope.componentes = [];
            $scope.idVersion = $routeParams.idVersion;
            $scope.componentesOficiales = [];
            $scope.modulos = [];

            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
            }).error(function (data) {
                console.debug(data);
            });

            serviceAdmin.getComponentesOficiales().success(function (data) {
                $scope.componentesOficiales = data;
            }).error(function (err) {
                console.error(err);
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

            $scope.ComponenteOkSegunExistencia = function (fileItem) {
                serviceAdmin.existeComponente(fileItem.file.name).success(function (data) {
                    fileItem.isError = !data;
                }).error(function (err) {
                    console.error(err);
                    fileItem.isError = false;
                });
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
                //console.info('onSuccessItem', fileItem);
                if (response.CodErr == 0) {
                    serviceAdmin
                        .addComponente($scope.idVersion, response.sModulo, fileItem.file.name, fileItem.file.lastModifiedDate.toISOString(), response.sVersion)
                        .success(function (data) {
                            console.log(JSON.stringify(data));
                        })
                        .error(function (data) {
                            console.error(data);
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
                $scope.ComponenteOkSegunExistencia(fileItem);
            };

            //console.info('uploader', uploader);
        }
    }
})();
