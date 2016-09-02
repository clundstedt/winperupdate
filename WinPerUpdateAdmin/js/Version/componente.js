(function () {
    'use strict';

    angular
        .module('app')
        .controller('componente', componente);

    componente.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'FileUploader'];

    function componente($scope, $routeParams, serviceAdmin, FileUploader) {
        $scope.title = 'componente';

        activate();

        function activate() {
            $scope.componentes = [];
            $scope.idVersion = $routeParams.idVersion;

            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
            }).error(function (data) {
                console.debug(data);
            });

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
                        .addComponente($scope.idVersion, 'N/A', fileItem.file.name, fileItem.file.lastModifiedDate.toISOString(), response.sVersion)
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

            //console.info('uploader', uploader);
        }
    }
})();
