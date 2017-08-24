(function () {
    'use strict';

    angular
        .module('app')
        .controller('ambientes', ambientes);

    ambientes.$inject = ['$scope', 'serviceAmbientes', 'serviceSeguridad', 'FileUploader', '$window'];

    function ambientes($scope, serviceAmbientes, serviceSeguridad, FileUploader, $window) {
        $scope.title = 'Ambientes';

        activate();

        function activate() {
            $scope.msgError = "";
            $scope.msgSuccess = "";
            $scope.ambientes = [];
            $scope.ambientesxlsx = [];
            console.log("id User = " + $("#idToken").val());
            $scope.idUsuario = $("#idToken").val()
            $scope.ambxlsxwarn = false;

            serviceSeguridad.getUsuario($scope.idUsuario).success(function (data) {
                $scope.idCliente = data.Cliente.Id;
                serviceAmbientes.getAmbientes($scope.idCliente).success(function (ambientes) {
                    console.log(JSON.stringify(ambientes));
                    $scope.ambientes = ambientes;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
                serviceAmbientes.getAmbientesXlsx($scope.idCliente).success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].EstadoRegistro == 1) {
                            $scope.ambxlsxwarn = true;
                            break;
                        }
                    }
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            $scope.downloadFile = function () {
                window.location = '/api/AmbientesXLSX/Planilla';
            }

            $scope.CargarAmbXLSX = function () {
                $("#ambxlsx-modal").modal('show');
                serviceAmbientes.getAmbientesXlsx($scope.idCliente).success(function (data) {
                    $scope.ambientesxlsx = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            //Proceso de upload
            var uploadOptions = {
                url: '/AmbientesClt/Upload?idUsuario=' + $scope.idUsuario,
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
                    if (response.sCliente == "OK") {
                        serviceAmbientes.getAmbientes($scope.idCliente).success(function (ambientes) {
                            $scope.ambientes = ambientes;
                            $scope.ambxlsxwarn = false;
                            $scope.msgError = "";
                            $scope.msgSuccess = "Ambientes importados y agregados correctamente.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    } else {
                        $scope.ambxlsxwarn = true;
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

            //console.info('uploader', uploader);
            //fin upload
        }
    }
})();
