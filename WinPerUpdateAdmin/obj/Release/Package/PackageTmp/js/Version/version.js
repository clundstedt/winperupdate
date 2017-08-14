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


            $scope.msgError = "";
            $scope.msgSuccess = "";

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
            $scope.version = null;
            $scope.componenteAEliminar = "";
            $scope.listaControlCambios = [];
            $scope.listaModCCFaltantes = [];

            if (!jQuery.isEmptyObject($routeParams)) {
                serviceAdmin.getComponentesOficiales().success(function (data) {
                    $scope.componentesOficiales = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.formData.release = data.Release;
                    $scope.formData.fecha = data.FechaFmt;
                    $scope.formData.estado = data.Estado;
                    $scope.formData.hasDeploy = data.HasDeploy31;
                    $scope.version = data;
                    serviceAdmin.getComponentesVersion(data.IdVersion).success(function (dataCV) {
                        $scope.componentes = dataCV;
                    }).error(function (errCV) {
                        console.error(errCV);
                    });
                    $scope.formData.comentario = data.Comentario;
                    $scope.fechaini = data.FechaFmt;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

                serviceAdmin.getTiposComponentes($scope.idversion).success(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var datos = {
                            Tipo: data[i]
                        }
                        $scope.TipoComponentes.push(datos);
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

                serviceAdmin.getControlCambios($scope.idversion).success(function (data) {
                    $scope.listaControlCambios = data;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

                $scope.msgError = "";
            }else{
                serviceAdmin.getUltimaRelease().success(function (data) {
                    $scope.formData.release = $scope.GenerarNuevaVersion(data);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.debug(err); 
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

            $scope.showMdlPublicaModal = function(){
                serviceAdmin.ccOK($scope.idversion).success(function (data) {
                    if (data.length == 0) {
                        $("#publica-modal").modal('show');
                    } else {
                        $scope.listaModCCFaltantes = data;
                        $("#mdlModCCFaltantes").modal('show');
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                });
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
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.UpdateEstado = function (estadoVersion) {
                $scope.msgSuccess = "";
                serviceAdmin.updEstadoVersion($scope.idversion, estadoVersion).success(function (data) {
                    $scope.formData.estado = estadoVersion;
                    $scope.msgError = "";
                    $scope.msgSuccess = "Cambios realizados exitosamente!.";
                    $("#confirma-cambio").modal('toggle');
                    $timeout(function () {
                        window.scrollTo(0,0);
                    },3000);
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    $("#confirma-cambio").modal('toggle');
                });
            }

            $scope.SaveVersion = function (formData) {
                $scope.msgSuccess = "";
                if ($scope.idversion == 0) {
                    serviceAdmin.addVersion(formData.release, formData.fecha, 'N', formData.comentario, '').success(function (data) {
                        $scope.msgSuccess = "Versión creada exitosamente!.";
                        window.scrollTo(0,0);
                        $scope.msgError = "";
                        $timeout(function () {
                            $window.location.href = "/Admin#/EditVersion/" + data.IdVersion;
                        },3000);
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                }
                else {
                    serviceAdmin.updVersion($scope.idversion, formData.release, formData.fecha, 'N', formData.comentario, '', '', formData.hasDeploy).success(function (data) {
                        $scope.idversion = data.IdVersion;
                        $scope.msgError = "";
                        $scope.msgSuccess = "Versión modificada exitosamente!.";
                        window.scrollTo(0,0);
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                    if (data.CodErr == 0) {
                        $scope.mensaje = "Publicando Versión ...";
                        serviceAdmin.addClientesToVersion($scope.idversion, $scope.listaClientes, $scope.radioPublicar).success(function (dataAddCTV) {
                            serviceAdmin.getVersion($scope.idversion).success(function (data1) {
                                data1.Estado = 'P';
                                data1.Instalador = data.Output;
                                console.log(data1.Instalador);
                                serviceAdmin.updVersion($scope.idversion, data1.Release, data1.FechaFmt, data1.Estado, data1.Comentario, $scope.idUsuario, data1.Instalador).success(function (data2) {
                                    $scope.mensaje = "Versión Publicada exitosamente ";
                                    console.debug($scope.mensaje);
                                    $scope.formData.estado = data2.Estado;
                                    $scope.msgError = "";
                                }).error(function (err) {
                                    $scope.mensaje = "Hubo errores al actualizar los datos de la version. Ver consola del navegador.";
                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                                });
                            }).error(function (err) {
                                $scope.mensaje = "Hubo errores al obtener la información de la versión. Ver consola del navegador";
                                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                            });
                        }).error(function (errAddCTV) {
                            $scope.mensaje = "Hubo errores al publicar. Ver consola del navegador";
                            console.error(errAddCTV);
                        });
                    } else {
                        $scope.mensaje = "Hubo errores al generar la versión. Ver consola del navegador";
                        console.error("CodErr: " + data.CodErr + ". MsgErr: " + data.MsgErr);
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    $scope.mensaje = "Hubo errores al generar instalador. Ver consola del navegador";
                });

            }

            $scope.EliminandoComponente = function (componente) {
                $scope.componenteAEliminar = componente;
                $("#mdlEliminarComponente").modal('show');
            }

            $scope.EliminarComponente = function () {
                serviceAdmin.delComponente($scope.idversion, $scope.componenteAEliminar).success(function () {
                    $("#mdlEliminarComponente").modal('toggle');
                    location.reload();
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }
        }
    }
})();
