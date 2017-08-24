(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'serviceAmbientes', '$timeout', '$window'];

    function admin($scope, $routeParams, serviceAdmin, serviceAmbientes, $timeout, $window) {
        $scope.title = 'admin';

        activate();

        function activate() {
            
            $scope.msgError = "";
            $scope.msgSuccess = "";

            $scope.version = {};
            $scope.versiones = [];
            $scope.ambientes = [];
            $scope.totales = [0, 0, 0];
            $scope.idUsuario = $("#idToken").val();
            $scope.idAmbiente = 0;
            $scope.mensaje = "";
            $scope.showScript = false;
            $scope.ambienteSel = null;
            $scope.DetalleCambio = "";

            $scope.TipoComponentes = [];

            $scope.idAmbienteExSQL = -1;
            $scope.nombreambienteExSQL = "";
            $scope.moduloExSQL = "";
            $scope.nombrearchExSQL = "";
            $scope.idcltExSQL = -1;
            $scope.codprfExSQL = -1;
            $scope.msgAvisoExSQL = "";
            $scope.isTareaAtrasada = false;
            $scope.detalleTareas = [];
            $scope.labelReportar = "Enviar";
            $scope.labelReportarTodasTareas = "Reportar Todo";
            $scope.ambientesNoEjecutados = [];
            $scope.actAvisoTareasNoReportadas = false;
            $scope.errorTextArea = false;
            $scope.cliente = null;
            $scope.hasSql = false;

            $scope.listaControlCambios = [];

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idversion = $routeParams.idVersion;
                serviceAdmin.getHasScripts($scope.idversion).success(function (hasSql) {
                    $scope.hasSql = hasSql;
                    serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                        serviceAdmin.getAmbientes(cliente.Id, $scope.idversion).success(function (ambiente) {
                            serviceAdmin.getVersionCliente($scope.idversion, cliente.Id).success(function (dataVersion) {
                                serviceAdmin.getTiposComponentes($scope.idversion).success(function (data) {
                                    serviceAdmin.getControlCambios($scope.idversion).success(function (dataCC) {
                                        $scope.listaControlCambios = dataCC;
                                        for (var i = 0; i < data.length; i++) {
                                            var datos = {
                                                Tipo: data[i]
                                            }
                                            $scope.TipoComponentes.push(datos);
                                        }
                                        $scope.msgError = "";
                                        $scope.version = dataVersion;
                                        $scope.ambientes = ambiente;
                                        $scope.cliente = cliente;
                                    }).error(function (err) {
                                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                    });
                                }).error(function (err) {
                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                });
                            }).error(function (err) {
                                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                            });
                        }).
                        error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                        });
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                });
            }
            else {
                serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                    serviceAdmin.getVersiones(cliente.Id).success(function (data) {
                        angular.forEach(data, function (version) {
                            $scope.versiones.push(version);
                            if (version.Estado == 'P' && !version.isInstall) $scope.totales[0]++;
                            else if (version.isInstall && version.Estado == 'P') $scope.totales[1]++;
                            else if (version.Estado == 'C') $scope.totales[2]++;
                        });
                        $scope.msgError = "";
                    }).
                    error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                    });
                })
                .error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                });
            }


            $scope.EjecutadoEnPruebas = function (idAmbientes, paso)
            {
                var existePruebas = false;
                var ex = true;
                if (!$scope.isAmbientePrueba(idAmbientes)) {
                    for (var i = 0; i < $scope.ambientes.length; i++) {
                        if ($scope.ambientes[i].Tipo != 1) {
                            existePruebas = true;
                        }
                    }
                    if (existePruebas) {
                        for (var i = 0; i < $scope.ambientes.length; i++) {
                            if ($scope.ambientes[i].Tipo != 1 && $scope.ambientes[i].Estado  != 'V') {
                                ex = false;
                            }
                        }
                    }
                }
            
                if (!paso) ex = true;
                return ex;
            }

            $scope.ShowDetalle = function (comentario) {
                if (comentario == "-") {
                    $scope.DetalleCambio = "No existe detalle de cambios.";
                } else {
                    $scope.DetalleCambio = comentario;
                }
                $("#detallecomp-modal").modal('show');
            }

            $scope.isAmbientePrueba = function (idAmbiente) {
                for (var i = 0; i < $scope.ambientes.length; i++) {
                    if ($scope.ambientes[i].idAmbientes == idAmbiente && $scope.ambientes[i].Tipo != 1) {
                        return true;
                    }
                }
                return false;
            }

            $scope.ExistenTareasNoReportadas = function () {
                for (var i = 0; i < $scope.detalleTareas.length; i++) {
                    if (!$scope.detalleTareas[i].Reportado && ($scope.detalleTareas[i].Estado == 0 || $scope.detalleTareas[i].Estado == 2 || $scope.detalleTareas[i].Estado == 4)) {
                        return true;
                    }
                }
                return false;
            }

            $scope.GetAmbientesNoEx = function (idCliente, idVersion, Namefile) {
                serviceAdmin.getAmbientesNoEx(idCliente, idVersion, Namefile).success(function (data) {
                    $scope.ambientesNoEjecutados = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.ReportarTodasTareas = function () {
                if ($scope.ExistenTareasNoReportadas()) {
                    console.log($scope.detalleTareas);
                    $scope.labelReportarTodasTareas = "Enviando...";
                    serviceAdmin.reportarTodasTareas($scope.detalleTareas).success(function (data) {
                        if (data) {
                            for (var i = 0; i < $scope.detalleTareas.length; i++) {
                                if ($scope.detalleTareas[i].Estado == 0 || $scope.detalleTareas[i].Estado == 2 || $scope.detalleTareas[i].Estado == 4) {
                                    $scope.detalleTareas[i].Reportado = true;
                                }
                            }
                            $scope.labelReportarTodasTareas = "Reportados";
                        }
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                } else {
                    $scope.actAvisoTareasNoReportadas = true;
                }
            }

            $scope.ReportarTareaAtrasada = function (tarea) {
                $scope.labelReportar = "Enviando...";
                serviceAdmin.reportarTareaAtrasada(tarea).success(function (data) {
                    if (data) {
                        $scope.labelReportar = "";
                        tarea.Reportado = true;
                    }
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.AsignarEstadoManual = function (tarea, estado) {
                if (tarea.Error.trim().length > 0) {
                    tarea.Estado = estado;
                    if (tarea.Estado == 3) {
                        tarea.Error = "Error corregido por el cliente";
                    }

                    serviceAdmin.asignarEstadoManual(tarea).success(function (data) {
                        $("#detalletareas-modal").modal('toggle');
                        setTimeout(function () {
                            $scope.ShowDetalleTarea(tarea.idClientes, tarea.idVersion);
                        }, 1500);
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                } else {
                    $scope.actErrorTextArea = true;
                }
            }

            $scope.ShowDetalleTarea = function (idCliente, idVersion) {
                $scope.actAvisoTareasNoReportadas = false;
                $scope.actErrorTextArea = false;
                serviceAdmin.detalleTareas(idCliente, idVersion).success(function (data) {
                    $scope.detalleTareas = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
                $("#detalletareas-modal").modal('show');
            }

            $scope.CheckInstall = function (idVersion, idUsuario, idAmbiente) {
                serviceAdmin.getCheckInstall(idVersion, idUsuario, idAmbiente).success(function (dataCheck) {
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
                
            }

            $scope.publish = function (idVersion, idCliente, idAmbiente, paso, nombreAmbiente, codPrf, AmbienteSel) {
                if ($scope.version.Estado == 'P') {
                    serviceAdmin.getCheckInstall(idVersion, idCliente, idAmbiente).success(function (dataCheck) {
                        if (dataCheck) {
                            if ($scope.hasSql) {
                                serviceAdmin.getScriptsOk(idVersion, idCliente, idAmbiente).success(function (data) {
                                    if (data == 0) {
                                        if ($scope.EjecutadoEnPruebas(idAmbiente, paso)) {
                                            $scope.nombreambiente = nombreAmbiente;
                                            $scope.idAmbiente = idAmbiente;
                                            $scope.ambienteSel = AmbienteSel;
                                            $scope.estaVigente = false;
                                            $("#publish-modal").modal('show');
                                            $scope.msgError = "";
                                        } else {
                                            $scope.nombreambiente = nombreAmbiente;
                                            $scope.idAmbiente = idAmbiente;
                                            $scope.ambienteSel = AmbienteSel;
                                            $scope.msgAvisoExSQL = "Se ha detectado que usted tiene un ambiente de pruebas, WinAct debe validar primero que la versión tiene que ser publicada con exito en un ambiente de pruebas.";
                                            $("#confpub-modal").modal('show');
                                        }
                                    } else if (data == 1) {
                                        $("#mdlAvisoScriptPendiente").modal('show');
                                    } else if (data == 2) {
                                        $scope.idAmbiente = idAmbiente;
                                        $scope.ambienteSel = AmbienteSel;
                                        $("#mdlAddTareas").modal('show');
                                    } else {
                                        $scope.ambienteSel = AmbienteSel;
                                        $scope.ambienteSel.EstadoEjecucionSql = 3;
                                        $scope.ambienteSel.ColorEstadoEjecucionSql = "danger";
                                        $scope.ShowDetalleTarea(idCliente, idVersion);
                                    }
                                }).error(function (err) {
                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                                });
                            } else {
                                if ($scope.EjecutadoEnPruebas(idAmbiente, paso)) {
                                    $scope.nombreambiente = nombreAmbiente;
                                    $scope.idAmbiente = idAmbiente;
                                    $scope.ambienteSel = AmbienteSel;
                                    $scope.estaVigente = false;
                                    $("#publish-modal").modal('show');
                                    $scope.msgError = "";
                                } else {
                                    $scope.nombreambiente = nombreAmbiente;
                                    $scope.idAmbiente = idAmbiente;
                                    $scope.ambienteSel = AmbienteSel;
                                    $scope.msgAvisoExSQL = "Se ha detectado que usted tiene un ambiente de pruebas, WinAct debe validar primero que la versión tiene que ser publicada con exito en un ambiente de pruebas.";
                                    $("#confpub-modal").modal('show');
                                }
                            }
                        } else {
                            $("#checkins-modal").modal('show');
                        }
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                    });
                } else {
                    $("#avisocaduca-modal").modal('show');
                }
            }

            $scope.EjecutarSqls = function (idVersion, idCliente, idAmbiente, codPrf) {
                serviceAdmin.addTareas(idVersion, idCliente, idAmbiente, codPrf).success(function (data) {
                    console.log($scope.ambienteSel);
                    $("#mdlAddTareas").modal('toggle');
                    $scope.msgError = "";
                    $scope.msgSuccess = "Scripts programado para su ejecución, espere aviso de WinActUI para descargar los componentes de WinPer";
                    window.scrollTo(0, 0);
                    $scope.ambienteSel.EstadoEjecucionSql = 1;
                    $scope.ambienteSel.ColorEstadoEjecucionSql = "info";
                    $timeout(function () {
                        $scope.msgSuccess = "";
                    },5000);
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                });
            }

            $scope.ShowConfirmPublish = function (id, nombre, idVersion, paso) {
                if ($scope.version.Estado == 'P') {
                    serviceAdmin.getCheckInstall(idVersion, $scope.idUsuario, id).success(function (dataCheck) {
                        if (dataCheck) {
                            if ($scope.EjecutadoEnPruebas(id, paso)) {
                                serviceAdmin.ambienteOK(id, idVersion).success(function (data) {
                                    if (data) {
                                        $scope.nombreambiente = nombre;
                                        $scope.idAmbiente = id;
                                        $scope.estaVigente = false;
                                        $("#publish-modal").modal('show');
                                    } else {
                                        $scope.msgAvisoExSQL = "En este ambiente aun no se ejecutan los script SQL correspondientes. Estos script se pueden ejecutar de manera automática a través de WinAct o de forma manual.";
                                        $("#avisoexsql-modal").modal('show');
                                    }
                                    $scope.msgError = "";
                                }).error(function (err) {
                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                                });
                            } else {
                                $scope.nombreambiente = nombre;
                                $scope.idAmbiente = id;
                                $scope.msgAvisoExSQL = "Se ha detectado que usted tiene un ambiente de pruebas, WinAct debe validar primero que la versión tiene que ser publicada con exito en un ambiente de pruebas.";
                                $("#confpub-modal").modal('show');
                            }
                        } else {
                            $("#checkins-modal").modal('show');
                        }
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                } else {
                    $("#avisocaduca-modal").modal('show');
                }
            }

            $scope.ShowConfirmEjecSQL = function (id, nombre, modulo, nombrearch, idClt, CodPrf) {
                serviceAdmin.getCheckInstall($scope.idversion, $scope.idUsuario, id).success(function (dataCheck) {
                    if (dataCheck) {
                        $scope.idAmbienteExSQL = id;
                        $scope.nombreambienteExSQL = nombre;
                        $scope.moduloExSQL = modulo;
                        $scope.nombrearchExSQL = nombrearch;
                        $scope.idcltExSQL = idClt;
                        $scope.codprfExSQL = CodPrf;
                        $("#ejecsql-modal").modal('show');
                    } else {
                        $("#checkins-modal").modal('show');
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.EjecutarTareaSQL = function (idVersion, Estado) {
                $("#ejecsql-modal").modal('toggle');
                serviceAdmin.existeTarea($scope.idcltExSQL, $scope.idAmbienteExSQL, idVersion, $scope.nombrearchExSQL).success(function (data) {
                    if (!data) {
                        serviceAdmin.addTarea(idVersion, $scope.idcltExSQL, $scope.idAmbienteExSQL, $scope.codprfExSQL, $scope.moduloExSQL, $scope.nombrearchExSQL, Estado).success(function (data2) {
                            if (data2 == 1) {
                                if (Estado == 3) {
                                    $scope.msgAvisoExSQL = "WinperUpdate procederá a descargar el script en su ordenador dentro de unos segundos, usted debe ejecutarlo directamente en la base de datos. Si necesita descargar nuevamente el archivo puede hacerlo de la opción Estado de Tareas";
                                } else {
                                    $scope.msgAvisoExSQL = "El script ya fue programado, WinAct procederá a ejecutarlo en el ambiente seleccionado. Si usted confirmó la ejecución manual comenzará la descarga del archivo.";
                                }
                            }
                            $scope.msgError = "";
                        }).error(function (err) {
                            $scope.msgAvisoExSQL = "ERROR CONTROLADO: " + err;
                        });
                    } else {
                        $scope.msgAvisoExSQL = "El script ya fue programado en este ambiente y versión.";
                    }
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });


                if (Estado == 3) {
                    $("#confejscman-modal").modal('toggle');
                    setTimeout(function () {
                        $scope.downloadFile(idVersion, $scope.nombrearchExSQL);
                    }, 5000);
                    
                }
                $("#avisoexsql-modal").modal('show');
            }

            $scope.ShowScriptSQL = function (idVersion, NameFile) {
                serviceAdmin.getScript(idVersion, NameFile).success(function (data) {
                    $scope.showScript = true;
                    $scope.txtarea = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.downloadFile = function (idVersion, NameFile) {
                window.location = '/api/Version/' + idVersion + '/Componentes/' + NameFile + '/script';
            }

            $scope.Publicar = function () {
                serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                    serviceAdmin.addVersion($scope.idversion, cliente.Id, $scope.idAmbiente, 'V').success(function () {
                        $scope.mensaje = "Versión agregada exitosamente";
                        $scope.ambienteSel.EstadoEjecucionSql = 0;
                        $scope.ambienteSel.ColorEstadoEjecucionSql = "default";
                        angular.forEach($scope.ambientes, function (item) {
                            if (item.idAmbientes == $scope.idAmbiente) {
                                item.Estado = 'V';
                                $scope.estaVigente = true;
                            }
                        });
                        $scope.msgError = "";
                    }).
                    error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                })
                .error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }
        }
    }
})();


