(function () {
    'use strict';

    angular
        .module('app')
        .controller('version', version);

    version.$inject = ['$scope', '$window', '$routeParams', 'serviceAdmin'];

    function version($scope, $window, $routeParams, serviceAdmin) {
        $scope.title = 'version';

        activate();

        function activate() {
            $scope.idversion = 0;
            $scope.increate = true;
            $scope.titulo = "Crear Versión";
            $scope.labelcreate = "Crear";
            $scope.componentes = [];
            $scope.totales = [0, 0, 0];
            $scope.formData = {};
            $scope.fechaini = '';
            $scope.formData.estado = 'N';
            $scope.mensaje = '';

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idversion = $routeParams.idVersion;
                $scope.titulo = "Modificar Versión";
                $scope.labelcreate = "Modificar";

                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.formData.release = data.Release;
                    $scope.formData.fecha = data.FechaFmt;
                    $scope.formData.estado = data.Estado;
                    $scope.componentes = data.Componentes;
                    $scope.formData.comentario = data.Comentario;

                    $scope.fechaini = data.FechaFmt;

                    angular.forEach($scope.componentes, function (item) {
                        if (item.Tipo == 'exe') $scope.totales[0]++;
                        else if (item.Tipo == 'qrp') $scope.totales[1]++;
                        else $scope.totales[2]++;
                    });

                }).error(function (data) {
                    console.error(data);
                });
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.ShowConfirmPublish = function () {
                $("#publish-modal").modal('show');
            }

            $scope.Eliminar = function () {
                serviceAdmin.delVersion($scope.idversion).success(function () {
                    $('.close').click();

                    $window.setTimeout(function () {
                        $window.location.href = "/Admin#/";
                    }, 2000);

                }).error(function (data) {
                    console.debug(data);
                });
            }

            $scope.SaveVersion = function (formData) {
                if ($scope.idversion == 0) {
                    serviceAdmin.addVersion(formData.release, formData.fecha, 'N', formData.comentario, '').success(function (data) {
                        //console.log(JSON.stringify(data));
                        $window.location.href = "/Admin#/EditVersion/" + data.IdVersion;
                    }).error(function (data) {
                        console.error(data);
                    });
                }
                else {
                    serviceAdmin.updVersion($scope.idversion, formData.release, formData.fecha, 'N', formData.comentario, '', '').success(function (data) {
                        console.log(JSON.stringify(data));
                        $window.location.href = "/Admin#/EditVersion/" + data.IdVersion;
                    }).error(function (data) {
                        console.error(data);
                    });
                }
            };

            $scope.Publicar = function () {
                $scope.mensaje = "Generando Instalador ...";
                serviceAdmin.genVersion($scope.idversion).success(function (data) {
                    $scope.mensaje = "Publicando Versión ...";
                    serviceAdmin.getVersion($scope.idversion).success(function (data1) {
                        data1.Estado = 'P';
                        data1.Instalador = data.Output;
                        //console.log(JSON.stringify(data1));

                        serviceAdmin.updVersion($scope.idversion, data1.Release, data1.FechaFmt, data1.Estado, '', '', data1.Instalador).success(function (data2) {
                            $scope.mensaje = "Versión Publicada exitosamente ";
                            console.debug($scope.mensaje);
                            //console.log(JSON.stringify(data2));
                            $scope.formData.estado = data2.Estado;
                        }).error(function (data) {
                            $scope.mensaje("Hubo errores al publicar. Ver el Log");
                            console.error(data);
                        });

                    }).error(function (data) {
                        $scope.mensaje("Hubo errores al publicar. Ver el Log");
                        console.error(data);
                    });
                });
            }
        }

    }
})();
