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
            $scope.idUsuario = $("#idToken").val();
            $scope.btnBlock = true;
            $scope.componentesOficiales = [];

            

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
            }else{
                serviceAdmin.getUltimaRelease().success(function (data) {
                    $scope.formData.release = $scope.GenerarNuevaVersion(data);
                }).error(function (err) {
                    console.log(err);
                });
            }

            serviceAdmin.getComponentesOficiales().success(function (data) {
                $scope.componentesOficiales = data;
            }).error(function (err) {
                console.error(err);
            });

            $scope.ComponenteOkSegunVersion = function (file) {
                for (var i = 0; i < $scope.componentesOficiales.length; i++) {
                    if ($scope.componentesOficiales[i].Version != null && $scope.componentesOficiales[i].Name == file.Name) {
                        var versionBase = $scope.GenerarNuevaVersion($scope.componentesOficiales[i].Version);
                        var versionOtra = file.Version;
                        var comparacion = $scope.ComparaVersion(versionBase, versionOtra);
                        console.log("comparacion: " + comparacion);
                        if (comparacion == 0) {
                            return "success";
                        } else if (comparacion == 1) {
                            file.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente es " + file.Version + " y debiera ser " + versionBase + ", ya que la versión oficial es " + $scope.componentesOficiales[i].Version;
                            return "warning";
                        } else {
                            file.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente (" + file.Version + ") debiera ser " + versionBase + ".";
                            return "danger";
                        }
                    }
                }
                return "success";
            }

            $scope.ComponentesOkSegunVersion = function () {
                for (var i = 0; i < $scope.componentesOficiales.length; i++) {
                    if ($scope.componentesOficiales[i].Version != null) {
                        var versionBase = $scope.GenerarNuevaVersion($scope.componentesOficiales[i].Version);
                        for (var j = 0; j < $scope.componentes.length; j++) {
                            if ($scope.componentes[j].Version != null) {
                                var versionOtra = $scope.componentes[j].Version;
                                if (!isNaN($scope.VersionToInt(versionOtra))) {
                                    if ($scope.ComparaVersion(versionBase,versionOtra) == -1){
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }

            /*
            Retorna -1 Si VersionOtra es menor
            Retorna 0 Si las versiones son iguales
            Retorna 1 Si la VersionOtra es mayor
            */
            $scope.ComparaVersion = function (versionBase, versionOtra) {
                var arrVB = versionBase.split('.');
                var arrVO = versionOtra.split('.');
                do {
                    if (arrVB.length < arrVO.length) versionBase += ".0";
                    else if (arrVO.length < arrVB.length) versionOtra += ".0";
                    arrVB = versionBase.split('.');
                    arrVO = versionOtra.split('.');
                } while (arrVB.length != arrVO.length);
                for (var i = 0; i < arrVB.length; i++) {
                    if (parseInt(arrVB[i]) > parseInt(arrVO[i])) return -1;
                    else if (parseInt(arrVO[i]) > parseInt(arrVB[i])) return 1;
                }
                return 0;
            }

            $scope.VersionToInt = function(release){
                var array = release.split('.');
                var strV = "";
                for (var i = 0; i < array.length; i++) {
                    strV += array[i];
                }
                return parseInt(strV);
            }

            $scope.AumentarRelease = function (formData) {
                if ($scope.formData.release.trim().length != 0) {
                    var nVersion = "";
                    var parts = formData.release.split('.');
                    if (parts.length == 1) {
                        nVersion += (parseInt(parts[0]) + 1);
                    } else {
                        nVersion += parts[0] + ".";
                        nVersion += (parseInt(parts[1]) + 1);
                        for (var i = 2; i < parts.length; i++) {
                            nVersion += ".0";
                        }
                    }
                    $scope.formData.release = nVersion;
                }
            }

            $scope.GenerarNuevaVersion = function (versionActual) {
                var nVersion = "";
                var parts = versionActual.split('.');
                parts[parts.length - 1] = (parseInt(parts[parts.length - 1]) + 1) + "";
                for (var i = 0; i < parts.length; i++) {
                    nVersion += parts[i] + (i != parts.length -1 ? "." : "");
                }
                return nVersion;
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.ShowConfirmPublish = function () {
                if ($scope.ComponentesOkSegunVersion()) {
                    $("#publish-modal").modal('show');
                } else {
                    $("#avisocomOk-modal").modal('show');
                }

            }

            $scope.PublicarParcial = function (version) {                                                                     
                if ($scope.ComponentesOkSegunVersion()) {
                    window.location = "Admin#/PublicarParcial/" + version;
                } else {
                    $("#avisocomOk-modal").modal('show');
                }
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
                        serviceAdmin.updVersion($scope.idversion, data1.Release, data1.FechaFmt, data1.Estado, data1.Comentario, $scope.idUsuario, data1.Instalador).success(function (data2) {
                            $scope.mensaje = "Versión Publicada exitosamente ";
                            console.debug($scope.mensaje);
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
