(function () {
    'use strict';

    angular
        .module('app')
        .controller('version', version);

    version.$inject = ['$scope', '$window', '$routeParams', 'serviceAdmin', '$timeout'];

    function version($scope, $window, $routeParams, serviceAdmin, $timeout) {
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
            $scope.TipoComponentes = [];

            $scope.radioPublicar = 0;
            $scope.listaClientes = [];
            $scope.lblMsgPublica = "";
            

            if (!jQuery.isEmptyObject($routeParams)) {

                serviceAdmin.getComponentesOficiales().success(function (data) {
                    $scope.componentesOficiales = data;
                }).error(function (err) {
                    console.error(err);
                });
                $scope.idversion = $routeParams.idVersion;
                $scope.titulo = "Modificar Versión";
                $scope.labelcreate = "Modificar";

                serviceAdmin.getClientes().success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var cl = {
                            check: false,
                            cliente: data[i]
                        };
                        $scope.listaClientes.push(cl);
                    }
                }).error(function (err) {
                    console.error(err);
                });

                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.formData.release = data.Release;
                    $scope.formData.fecha = data.FechaFmt;
                    $scope.formData.estado = data.Estado;
                    serviceAdmin.getComponentesVersion(data.IdVersion).success(function (dataCV) {
                        $scope.componentes = dataCV;
                    }).error(function (errCV) {
                        console.error(errCV);
                    });

                    /*METODO DE VALIDACION DE COMPONENTE POR ANGULAR
                    for (var i = 0; i < data.Componentes.length; i++) {
                        var comp = {
                            isOK : "success",
                            componente: data.Componentes[i]
                        }
                        $scope.componentes.push(comp);
                    }
                    $timeout(function () {
                        for (var i = 0; i < $scope.componentes.length; i++) {
                            $scope.ComponenteOkSegunVersion($scope.componentes[i]);
                        }
                        console.log($scope.componentes);
                    }, 1500);*/
                    
                    $scope.formData.comentario = data.Comentario;

                    $scope.fechaini = data.FechaFmt;


                }).error(function (data) {
                    console.error(data);
                });

                serviceAdmin.getTiposComponentes($scope.idversion).success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var datos = {
                            Tipo: data[i]
                        }
                        $scope.TipoComponentes.push(datos);
                    }
                }).error(function (err) {
                    console.error(err);
                });
            }else{
                serviceAdmin.getUltimaRelease().success(function (data) {
                    $scope.formData.release = $scope.GenerarNuevaVersion(data);
                }).error(function (err) {
                    console.log(err);
                });
            }


            $scope.ComponenteOkSegunVersion = function (file) {
                for (var i = 0; i < $scope.componentesOficiales.length; i++) {
                    if ($scope.componentesOficiales[i].Version != null && $scope.componentesOficiales[i].Name == file.componente.Name) {
                        var versionBase = $scope.GenerarNuevaVersion($scope.componentesOficiales[i].Version);
                        var versionOtra = file.componente.Version;
                        var comparacion = $scope.ComparaVersion(versionBase, versionOtra);
                        console.log("comparacion: " + comparacion);
                        if (comparacion == 0) {
                            file.isOk = "success";
                        } else if (comparacion == 1) {
                            file.componente.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente es " + file.componente.Version + " y debiera ser " + versionBase + ", ya que la versión oficial es " + $scope.componentesOficiales[i].Version;
                            file.isOk = "warning";
                        } else {
                            file.componente.MensajeToolTip = "WinperUpdate ha detectado que la versión de este componente (" + file.componente.Version + ") debiera ser " + versionBase + ".";
                            file.isOk = "danger";
                        }
                        break;
                    } else {
                        file.isOk = "success";
                    }
                }
                return file;
            }

            $scope.ComponentesOkSegunVersion = function () {
                for (var i = 0; i < $scope.componentes.length; i++) {
                    if($scope.componentes[i].isOk == "danger"){
                        return false;
                    }
                }
                return true;
            }

            $scope.OmitirComponentes = function () {
                for (var i = 0; i < $scope.componentes.length; i++) {
                    if ($scope.componentes[i].isOk == "danger") {
                        $scope.componentes[i].isOk = "success";
                    }
                }
                $("#confirmeomitir-modal").modal('toggle');
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
                if ($scope.VerificaSeleccionCliente()) {
                    $("#publica-modal").modal('toggle');
                    if ($scope.ComponentesOkSegunVersion()) {
                        $("#publish-modal").modal('show');
                    } else {
                        $("#avisocomOk-modal").modal('show');
                    }
                } else {
                    $scope.lblMsgPublica = "Debe seleccionar almenos un cliente.";
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

            $scope.UpdateEstado = function (estadoVersion) {
                serviceAdmin.updEstadoVersion($scope.idversion, estadoVersion).success(function (data) {
                    //$scope.formData.estado = estadoVersion;
                    $window.setTimeout(function () {
                        $window.location.href = "/Admin#/";
                    }, 2000);
                }).error(function (err) {
                    console.error(err);
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

            $scope.VerificaSeleccionCliente = function () {
                if ($scope.radioPublicar == 0) return true;
                for (var i = 0; i < $scope.listaClientes.length; i++) {
                    if($scope.listaClientes[i].check){
                        return true;
                    }
                }
                return false;
            }

            $scope.Publicar = function () {
                $scope.mensaje = "Generando Instalador ...";
                serviceAdmin.genVersion($scope.idversion).success(function (data) {
                    $scope.mensaje = "Publicando Versión ...";
                    serviceAdmin.addClientesToVersion($scope.idversion, $scope.listaClientes, $scope.radioPublicar).success(function (dataAddCTV) {
                        serviceAdmin.getVersion($scope.idversion).success(function (data1) {
                            data1.Estado = 'P';
                            data1.Instalador = data.Output;
                            console.log(data1.Instalador);
                            if (data.CodErr == 0) {
                                serviceAdmin.updVersion($scope.idversion, data1.Release, data1.FechaFmt, data1.Estado, data1.Comentario, $scope.idUsuario, data1.Instalador).success(function (data2) {
                                    $scope.mensaje = "Versión Publicada exitosamente ";
                                    console.debug($scope.mensaje);
                                    $scope.formData.estado = data2.Estado;
                                }).error(function (data) {
                                    $scope.mensaje = "Hubo errores al actualizar los datos de la version. Ver consola del navegador.";
                                    console.error(data);
                                });
                            } else {
                                $scope.mensaje = "Hubo errores al publicar. Ver consola del navegador";
                                console.error("CodErr: " + data.CodErr + ". MsgErr: " + data.MsgErr);
                            }
                        }).error(function (data) {
                            $scope.mensaje = "Hubo errores al obtener la información de la versión. Ver consola del navegador";
                            console.error(data);
                        });
                    }).error(function (errAddCTV) {
                        $scope.mensaje = "Hubo errores al publicar. Ver consola del navegador";
                        console.error(errAddCTV);
                    });
                }).error(function (err) {
                    console.error(err);
                    $scope.mensaje = "Hubo errores al generar instalador. Ver consola del navegador";
                });

            }
        }

    }
})();
