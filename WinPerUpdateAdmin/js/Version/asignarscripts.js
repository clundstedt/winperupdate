
(function () {
    'use strict';

    angular
        .module('app')
        .controller('asignarscripts', asignarscripts);

    asignarscripts.$inject = ['$scope', '$window', '$routeParams', 'serviceAdmin', '$timeout', 'FileUploader'];

    function asignarscripts($scope, $window, $routeParams, serviceAdmin, $timeout, FileUploader) {
        $scope.title = 'titulo';

        activate();

        function activate() {
            window.scrollTo(0, 0);
            $scope.motoresBd = 
                [{ cod: 1, nombre: "SQL Server" }
                , { cod: 2, nombre: "Oracle" }];
            $scope.modulos = [];
            $scope.version = null;
            $scope.msgError = "";

            if (!jQuery.isEmptyObject($routeParams)) {
                serviceAdmin.getVersion($routeParams.idVersion).success(function (data) {
                    $scope.version = data;
                    serviceAdmin.getModulos().success(function (modulos) {
                        $scope.msgError = "";
                        $scope.modulos = modulos;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0); window.scrollTo(0, 0);
                });
            }

            $scope.uploadAll = function () {
                $scope.uploaderAlt.uploadAll();
                $scope.uploaderSp.uploadAll();
                $scope.uploaderFn.uploadAll();
                $scope.uploaderTr.uploadAll();
            }

            $scope.cancelAll = function () {
                $scope.uploaderAlt.cancelAll();
                $scope.uploaderSp.cancelAll();
                $scope.uploaderFn.cancelAll();
                $scope.uploaderTr.cancelAll();
            }

            $scope.removeAll = function () {
                $scope.uploaderAlt.clearQueue();
                $scope.uploaderSp.clearQueue();
                $scope.uploaderFn.clearQueue();
                $scope.uploaderTr.clearQueue();
            }

            //ALters
            var uploaderAlt = $scope.uploaderAlt = new FileUploader();


            // FILTERS

            // a sync filter
            uploaderAlt.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
                }
            });

            // an async filter
            uploaderAlt.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            // CALLBACKS
            uploaderAlt.onSuccessItem = function (fileItem, response, status, headers) {
                //console.info("onSuccessItem", fileItem, response, status, headers);
            };
            uploaderAlt.onErrorItem = function (fileItem, response, status, headers) {
                //console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploaderAlt.onCancelItem = function (fileItem, response, status, headers) {
                //console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploaderAlt.onCompleteItem = function (fileItem, response, status, headers) {
                //console.info('onCompleteItem', fileItem, response, status, headers);
            };

            uploaderAlt.onAfterAddingFile = function (fileItem) {
                $scope.msgError = "";
                var fiSplit = fileItem.file.name.split('.');
                if (fiSplit[fiSplit.length - 1] != "sql" && fiSplit[fiSplit.length - 1] != "SQL") {
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
                fileItem.url = '';
            };

            uploaderAlt.onCompleteAll = function () {
                
            }

            //Sp
            var uploaderSp = $scope.uploaderSp = new FileUploader();


            // FILTERS

            // a sync filter
            uploaderSp.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
                }
            });

            // an async filter
            uploaderSp.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            // CALLBACKS
            uploaderSp.onSuccessItem = function (fileItem, response, status, headers) {
                
                //console.info("onSuccessItem", fileItem, response, status, headers);
            };
            uploaderSp.onErrorItem = function (fileItem, response, status, headers) {
                //console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploaderSp.onCancelItem = function (fileItem, response, status, headers) {
                //console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploaderSp.onCompleteItem = function (fileItem, response, status, headers) {
                //console.info('onCompleteItem', fileItem, response, status, headers);
            };

            uploaderSp.onAfterAddingFile = function (fileItem) {
                $scope.msgError = "";
                var fiSplit = fileItem.file.name.split('.');
                if (fiSplit[fiSplit.length - 1] != "sql" && fiSplit[fiSplit.length - 1] != "SQL") {
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
                fileItem.url = '';
            };

            uploaderSp.onCompleteAll = function () {

            }

            //Fn
            var uploaderFn = $scope.uploaderFn = new FileUploader();


            // FILTERS

            // a sync filter
            uploaderFn.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
                }
            });

            // an async filter
            uploaderFn.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            // CALLBACKS
            uploaderFn.onSuccessItem = function (fileItem, response, status, headers) {
                $scope.nameFiles.push(fileItem.file.name);
                //console.info("onSuccessItem", fileItem, response, status, headers);
            };
            uploaderFn.onErrorItem = function (fileItem, response, status, headers) {
                //console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploaderFn.onCancelItem = function (fileItem, response, status, headers) {
                //console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploaderFn.onCompleteItem = function (fileItem, response, status, headers) {
                //console.info('onCompleteItem', fileItem, response, status, headers);
            };

            uploaderFn.onAfterAddingFile = function (fileItem) {
                $scope.msgError = "";
                var fiSplit = fileItem.file.name.split('.');
                if (fiSplit[fiSplit.length - 1] != "sql" && fiSplit[fiSplit.length - 1] != "SQL") {
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
                fileItem.url = '';
            };

            uploaderFn.onCompleteAll = function () {

            }

            //Tr
            var uploaderTr = $scope.uploaderTr = new FileUploader();


            // FILTERS

            // a sync filter
            uploaderTr.filters.push({
                name: 'maxlenQueue',
                fn: function (item, options) {
                    return this.queue.length < 30;
                }
            });

            // an async filter
            uploaderTr.filters.push({
                name: 'sizeFilter',
                fn: function (item) {
                    return item.size < 52428800; // 50 Mbytes
                }
            });

            // CALLBACKS
            uploaderTr.onSuccessItem = function (fileItem, response, status, headers) {
                $scope.nameFiles.push(fileItem.file.name);
                //console.info("onSuccessItem", fileItem, response, status, headers);
            };
            uploaderTr.onErrorItem = function (fileItem, response, status, headers) {
                //console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploaderTr.onCancelItem = function (fileItem, response, status, headers) {
                //console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploaderTr.onCompleteItem = function (fileItem, response, status, headers) {
                //console.info('onCompleteItem', fileItem, response, status, headers);
            };

            uploaderTr.onAfterAddingFile = function (fileItem) {
                $scope.msgError = "";
                var fiSplit = fileItem.file.name.split('.');
                if (fiSplit[fiSplit.length - 1] != "sql" && fiSplit[fiSplit.length - 1] != "SQL") {
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
                fileItem.url = '';
            };

            uploaderTr.onCompleteAll = function () {

            }
        }

    }
})();
