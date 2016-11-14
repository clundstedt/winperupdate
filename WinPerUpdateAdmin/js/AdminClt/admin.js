(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', '$routeParams', 'serviceAdmin', 'serviceAmbientes'];

    function admin($scope, $routeParams, serviceAdmin, serviceAmbientes) {
        $scope.title = 'admin';

        activate();

        function activate() {
            $scope.version = {};
            $scope.versiones = [];
            $scope.ambientes = [];
            $scope.totales = [0, 0, 0];
            $scope.idUsuario = $("#idToken").val();
            $scope.idAmbiente = 0;
            $scope.mensaje = "";
            $scope.showScript = false;

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

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idversion = $routeParams.idVersion;
                $scope.titulo = "Modificar Versión";
                $scope.labelcreate = "Modificar";



                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.version = data;
                    $scope.version.Estado = 'N';

                    angular.forEach($scope.version.Componentes, function (item) {
                        if (item.Tipo == 'exe') $scope.totales[0]++;
                        else if (item.Tipo == 'qrp') $scope.totales[1]++;
                        else $scope.totales[2]++;
                    });

                    serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                        //console.log(JSON.stringify(cliente));
                        serviceAdmin.getAmbientes(cliente.Id, $scope.idversion).success(function (ambiente) {
                            $scope.ambientes = ambiente;
                        }).
                        error(function (err) {
                            console.error(err);
                        });

                        serviceAdmin.existeTareaAtrasada(cliente.Id, $scope.idversion).success(function (data) {
                            $scope.existeTareaAtrasada = data;
                            console.log($scope.existeTareaAtrasada);
                        }).error(function (err) {
                            console.log(err);
                        });
                        
                    })
                    .error(function (err) {
                        console.error(err);
                    });

                }).error(function (err) {
                    console.error(err);
                });
            }
            else {
                serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                    //console.log(JSON.stringify(cliente));
                    serviceAdmin.getVersiones(cliente.Id).success(function (data) {
                        angular.forEach(data, function (version) {
                            $scope.versiones.push(version);
                            if (version.Estado == 'P' || version.Estado == 'C') $scope.totales[0]++;
                            else if (version.Estado == 'N') $scope.totales[1]++;
                        });
                    }).
                    error(function (data) {
                        console.error(data);
                    });
                })
                .error(function (err) {
                    console.error(err);
                });
            }

            $scope.EjecutadoEnPruebas = function (idAmbientes)
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
            

                return ex;
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
                }).error(function (err) {
                    console.log(err);
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
                    }).error(function (err) {
                        console.log(err);
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
                }).error(function (err) {
                    console.log(err);
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
                    }).error(function (err) {
                        console.error(err);
                    });
                } else {
                    $scope.actErrorTextArea = true;
                }
            }

            $scope.ShowDetalleTarea = function (idCliente, idVersion) {
                serviceAdmin.detalleTareas(idCliente, idVersion).success(function (data) {
                    
                    $scope.detalleTareas = data;
                    
                }).error(function (err) {
                    console.log(err);
                });
                $("#detalletareas-modal").modal('show');
            }

            $scope.ShowConfirmPublish = function (id, nombre, idVersion) {
                if ($scope.EjecutadoEnPruebas(id)) {
                    serviceAdmin.ambienteOK(id, idVersion).success(function (data) {
                        if (data) {
                            $scope.nombreambiente = nombre;
                            $scope.idAmbiente = id;
                            $scope.estaVigente = false;
                            $("#publish-modal").modal('show');
                        } else {
                            $scope.msgAvisoExSQL = "En este ambiente aun no se ejecutan los script SQL correspondientes. Estos script se pueden ejecutar de manera automática a través de WinperUpdate o de forma manual.";
                            $("#avisoexsql-modal").modal('show');
                        }
                    }).error(function (err) {
                        console.log(err);
                    });
                } else {
                    $scope.msgAvisoExSQL = "Se ha detectado que usted tiene un ambiente de pruebas, WinperUpdate debe validar primero que la versión tiene que ser publicada con exito en un ambiente de pruebas.";
                    $("#avisoexsql-modal").modal('show');
                }
            }

            $scope.ShowConfirmEjecSQL = function (id, nombre,modulo,  nombrearch, idClt, CodPrf) {
                $scope.idAmbienteExSQL = id;
                $scope.nombreambienteExSQL = nombre;
                $scope.moduloExSQL = modulo;
                $scope.nombrearchExSQL = nombrearch;
                $scope.idcltExSQL = idClt;
                $scope.codprfExSQL = CodPrf;
                $("#ejecsql-modal").modal('show');
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
                                    $scope.msgAvisoExSQL = "El script ya fue programado, WinperUpdate procederá a ejecutarlo en el ambiente seleccionado. Si usted confirmó la ejecución manual comenzará la descarga del archivo.";
                                }
                            }
                        }).error(function (err) {
                            $scope.msgAvisoExSQL = "ERROR CONTROLADO: " + err;
                        });
                    } else {
                        $scope.msgAvisoExSQL = "El script ya fue programado en este ambiente y versión.";
                    }
                }).error(function (err) {
                    console.log(err);
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
                }).error(function (err) {
                    console.log(err);
                });
            }

            $scope.downloadFile = function (idVersion, NameFile) {
                window.location = '/api/Version/' + idVersion + '/Componentes/' + NameFile + '/script';
            }

            $scope.Publicar = function () {
                serviceAdmin.getCliente($scope.idUsuario).success(function (cliente) {
                    //console.log(JSON.stringify(cliente));
                    serviceAdmin.addVersion($scope.idversion, cliente.Id, $scope.idAmbiente, 'V').success(function () {
                        $scope.mensaje = "Versión agregada exitosamente";
                        angular.forEach($scope.ambientes, function (item) {
                            if (item.idAmbientes == $scope.idAmbiente) {
                                item.Estado = 'V';
                                $scope.estaVigente = true;
                            }
                        });
                    }).
                    error(function (err) {
                        console.error(err);
                    });
                })
                .error(function (err) {
                    console.error(err);
                });
            }
        }
    }
})();


