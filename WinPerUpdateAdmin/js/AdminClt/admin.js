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
            $scope.tareasAtrasadas = [];
            $scope.labelReportar = "Enviar";
            $scope.labelReportarTodasTareas = "Reportar Todo";

            

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

            $scope.ExistenTareasNoReportadas = function () {
                for (var i = 0; i < $scope.tareasAtrasadas.length; i++) {
                    if (!$scope.tareasAtrasadas[i].Reportado) {
                        return true;
                    }
                }
                return false;
            }

            $scope.ReportarTodasTareas = function () {
                if ($scope.ExistenTareasNoReportadas()) {
                    console.log($scope.tareasAtrasadas);
                    $scope.labelReportarTodasTareas = "Enviando...";
                    serviceAdmin.reportarTodasTareas($scope.tareasAtrasadas).success(function (data) {
                        if (data) {
                            for (var i = 0; i < $scope.tareasAtrasadas.length; i++) {
                                $scope.tareasAtrasadas[i].Reportado = true;
                            }
                            $scope.labelReportarTodasTareas = "Reportados";
                        }
                    }).error(function (err) {
                        console.log(err);
                    });
                } else {
                    $scope.labelReportarTodasTareas = "Reportados";
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

            $scope.ShowDetalleTarea = function (idCliente, idVersion) {
                serviceAdmin.detalleTareasNoEx(idCliente, idVersion).success(function (data) {
                    $scope.tareasAtrasadas = data;
                    
                }).error(function (err) {
                    console.log(err);
                });
                $("#detalletareas-modal").modal('show');
            }

            $scope.ShowConfirmPublish = function (id, nombre, idVersion) {
                serviceAdmin.ambienteOK(id, idVersion).success(function (data) {
                    if (data) {
                        $scope.nombreambiente = nombre;
                        $scope.idAmbiente = id;
                        $scope.estaVigente = false;
                        $("#publish-modal").modal('show');
                    } else {
                        $scope.msgAvisoExSQL = "En este ambiente deben ejecutarse correctamente los script SQL antes de publicar.";
                        $("#avisoexsql-modal").modal('show');
                    }
                }).error(function (err) {
                    console.log(err);
                });
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

            $scope.EjecutarTareaSQL = function (idVersion) {
                $("#ejecsql-modal").modal('toggle');

                serviceAdmin.existeTarea($scope.idcltExSQL, $scope.idAmbienteExSQL, idVersion, $scope.nombrearchExSQL).success(function (data) {
                    if (!data) {
                        serviceAdmin.addTarea(idVersion, $scope.idcltExSQL, $scope.idAmbienteExSQL, $scope.codprfExSQL, $scope.moduloExSQL, $scope.nombrearchExSQL).success(function (data) {
                            if (data == 1) {
                                $scope.msgAvisoExSQL = "El script ya fue programado, WinperUpdate procederá a ejecutarlo en el ambiente seleccionado.";
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


