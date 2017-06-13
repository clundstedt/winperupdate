(function () {
    'use strict';

    angular
        .module('app')
        .controller('modulos', modulos);

    modulos.$inject = ['$scope', '$routeParams','serviceModulos','$timeout'];

    function modulos($scope, $routeParams, serviceModulos, $timeout) {
        $scope.msgError = "";

        $scope.title = 'modulos';
        $scope.formData = {};
        $scope.formComp = {};
        $scope.formTipoComp = {};
        $scope.idModulo = 0;
        $scope.usuario = $("#idToken").val();
        $scope.loading = false;

        $scope.lblAvisoWinper = "";

        $scope.tipoalert = "";
        $scope.msgalert = "";

        $scope.accion = "Crear";

        //Componentes
        $scope.componentes = [];
        $scope.tipocomponentes = [];
        $scope.CreandoComponentesModulos = false;
        $scope.listaCompsDetect = [];
        $scope.lblAddComponentesDir = "";

        $scope.ModificarTipo = 0;
        $scope.lblModificarTipoComponente = "Modificar";

        $scope.suites = [];


        activate();

        function activate() {

            $scope.dirModulo = {};
            $scope.msgError = "";
            $scope.dirsModulos = [];
            $scope.isLoadDirs = false;

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idModulo = $routeParams.idModulo;
                serviceModulos.getModulo($scope.idModulo).success(function (data) {
                    $scope.formData.nombre = data.NomModulo;
                    $scope.formData.descripcion = data.Descripcion;
                    $scope.formData.iscore = data.isCore;
                    $scope.formData.directorio = data.Directorio;
                    $scope.formData.estado = data.Estado;
                    $scope.formData.suite = data.Suite;
                    $scope.accion = "Modificar";
                    serviceModulos.getComponentesModulo($scope.idModulo).success(function (data) {
                        $scope.componentes = data;
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }


            serviceModulos.getSuites().success(function (data) {
                $scope.suites = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            serviceModulos.getTipoComponentes().success(function (data) {
                $scope.tipocomponentes = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            $scope.showMdlDirModulos = function () {
                $scope.dirModulo = "";
                $scope.isLoadDirs = true;
                $("#mdlDirModulos").modal('show');
                serviceModulos.getDirs("N+1").success(function (data) {
                    $scope.dirsModulos = data;
                    $scope.isLoadDirs = false;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.cargarDirs = function (dir) {
                $scope.isLoadDirs = true;
                serviceModulos.getDirs(dir).success(function (data) {
                    $scope.dirsModulos = data;
                    $scope.dirModulo = dir.replace(dir.substring(0, dir.indexOf("N+1") + 4),"");
                    $scope.isLoadDirs = false;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.dirAnterior = function (dir) {
                if (dir.lastIndexOf("\\") != -1) {
                    $scope.dirModulo = dir.substring(0, dir.lastIndexOf("\\"));
                    $scope.cargarDirs("N+1\\"+$scope.dirModulo);
                } else {
                    $scope.cargarDirs("N+1");
                }
            }

            $scope.seleccionarDir = function (dir) {
                if (dir.trim().length == 0) {
                    $scope.msgError = "Seleccione un directorio.";
                } else {
                    $scope.formData.directorio = dir;
                    $scope.msgError = "";
                }
                $("#mdlDirModulos").modal('toggle');
            }

            $scope.CargarTipoComponentes = function () {
                serviceModulos.getTipoComponentes().success(function (data) {
                    $scope.tipocomponentes = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.CargarComponentesModulos = function () {
                serviceModulos.getComponentesModulo($scope.idModulo).success(function (data) {
                    $scope.componentes = data;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }
            
            $scope.CrearComponentesModulos = function (formComp) {
                serviceModulos.addComponentesModulos(formComp.nombre, formComp.descripcion, $scope.idModulo, formComp.tipocomponente).success(function (data) {
                    $scope.CargarComponentesModulos();
                    $("#mancom-modal").modal('toggle');
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.ModificarComponentesModulos = function (componenteModulo) {
                componenteModulo.LblModificar = "Modificando...";
                serviceModulos.updComponentesModulos(componenteModulo.idComponentesModulos, componenteModulo.Nombre, componenteModulo.Descripcion, componenteModulo.TipoComponentes.idTipoComponentes).success(function (data) {
                    componenteModulo.LblModificar = "Modificado!";
                    $timeout(function () {
                        componenteModulo.LblModificar = "Modificar";
                    }, 3000);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }
            
            $scope.CrearTipoComponente = function (formTipoComp) {
                serviceModulos.addTipoComponentes(formTipoComp.nombre, formTipoComp.iscompbd, formTipoComp.iscompdll, $scope.PreparaExtTipoComponente(formTipoComp.extensiones)).success(function (data) {
                    $scope.CargarTipoComponentes();
                    formTipoComp.nombre = "";
                    formTipoComp.extensiones = "";
                    formTipoComp.iscompbd = false;
                    formTipoComp.iscompdll = false;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.PreparaExtTipoComponente = function (extension) {
                var exts = extension.split(" ");
                var extFmt = "";
                for (var i = 0; i < exts.length; i++) {
                    if (!exts[i].startsWith(".")) exts[i] = "." + exts[i];
                    extFmt += exts[i] + " ";
                }
                return extFmt;
            }

            $scope.EliminarTipoComponente = function (tipoComponente) {
                if (tipoComponente.LblEliminarTipo == "Eliminar") {
                    tipoComponente.LblEliminarTipo = "Confirme!";
                    $timeout(function () {
                        tipoComponente.LblEliminarTipo = "Eliminar";
                    }, 3000);
                } else {
                    serviceModulos.delTipoComponentes(tipoComponente.idTipoComponentes).success(function (data) {
                        $scope.CargarTipoComponentes();
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
            }

            $scope.EliminarComponentesModulos = function (componentesModulos) {
                if (componentesModulos.LblEliminar == "Eliminar") {
                    componentesModulos.LblEliminar = "Confirme!";
                    $timeout(function () {
                        componentesModulos.LblEliminar = "Eliminar";
                    }, 3000);
                } else {
                    serviceModulos.delComponentesModulos(componentesModulos.idComponentesModulos).success(function (data) {
                        $scope.CargarComponentesModulos();
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
            }

            $scope.AgregarCompsDir = function () {
                if ($scope.listaCompsDetect.length > 0) {
                    serviceModulos.addComponentesDir($scope.listaCompsDetect).success(function (data) {
                        serviceModulos.GetComponentesDirectorio($scope.idModulo).success(function (data) {
                            $scope.listaCompsDetect = [];
                            for (var i = 0; i < data.length; i++) {
                                var compD = {
                                    check: false,
                                    componente: data[i]
                                }
                                $scope.listaCompsDetect.push(compD);
                            }
                            $scope.lblAddComponentesDir = "Componentes agregados correctamente.";
                            $scope.msgError = "";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                            $scope.lblAddComponentesDir = "Ocurrió un error durante la recarga de componentes, verifique consola del navegador.";
                        });
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                        $scope.lblAddComponentesDir = "Ocurrió un error, verifique consola del navegador.";
                    });
                }
            }

            $scope.SelectAllCompDir = function (SellAll) {
                for (var i = 0; i < $scope.listaCompsDetect.length; i++) {
                    $scope.listaCompsDetect[i].check = SellAll;
                }
            }

            $scope.LoadCompDetect = function () {
                serviceModulos.GetComponentesDirectorio($scope.idModulo).success(function (data) {
                    $scope.listaCompsDetect = [];
                    for (var i = 0; i < data.length; i++) {
                        var compD = {
                            check: false,
                            componente: data[i]
                        }
                        $scope.listaCompsDetect.push(compD);
                    }
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.Accion = function (formData) {
                serviceModulos.ExistDirModulo(formData.directorio).success(function (data) {
                    if (data) {
                        if ($scope.accion == "Crear") {
                            $scope.Crear(formData);
                        } else if ($scope.accion == "Modificar") {
                            $scope.Modificar(formData);
                        }
                    } else {
                        $scope.lblAvisoWinper = "El directorio del módulo no existe.";
                        $("#aviso-modal").modal('show');
                    }
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.Crear = function (formData) {
                $scope.loading = true;
                serviceModulos.addModulo(formData.nombre, formData.descripcion, formData.iscore, formData.directorio, formData.suite).success(function (data) {
                    $scope.accion = "Modificar";
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo creado exitosamente!.";
                    $scope.idModulo = data.idModulo;
                    $scope.formData.estado = data.Estado;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de creación del módulo, vuelva a intentarlo.";
                });
                $scope.loading = false;
                $timeout(function () {
                    $scope.tipoalert = "";
                }, 10000);
            }

            $scope.Modificar = function (formData) {
                $scope.loading = true;
                serviceModulos.updModulo($scope.idModulo, formData.nombre, formData.descripcion, formData.iscore, formData.directorio, formData.suite).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo modificado exitosamente!.";
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    $scope.tipoalert = "danger";
                    $scope.msgalert="Ocurrió un error durante el proceso de modificación, vuelva a intentarlo."
                });
                $scope.loading = false;
                $timeout(function () {
                    $scope.tipoalert = "";
                }, 10000);
            }

            $scope.Eliminar = function () {
                $scope.loading = true;
                serviceModulos.delModulo($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo fue caducado con exito!.";
                    $scope.formData.estado = 'C';
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de caducación del modulo, vuelva a intentarlo.";
                });
                $("#confelim-modal").modal('toggle');
                $scope.loading = false;
                $timeout(function () {
                    $scope.tipoalert = "";
                }, 10000);
            }

            $scope.Vigente = function () {
                $scope.loading = true;
                serviceModulos.setVigente($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo establecido como vigente exitosamente!.";
                    $scope.formData.estado = 'V';
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso, vuelva a intentarlo.";
                });
                $("#confvigen-modal").modal('toggle');
                $scope.loading = false;
                $timeout(function () {
                    $scope.tipoalert = "";
                }, 10000);
            }

            $scope.ShowManTipCom = function () {
                $("#mancom-modal").modal('hide');
                $("#mantipcom-modal").modal({ backdrop: 'static', keyboard: false, show: true });
                $scope.CreandoComponentesModulos = true;
                $scope.LimpiarManTipCom();
            }

            $scope.CreandoComponentes = function () {
                if ($scope.CreandoComponentesModulos) {
                    $("#mancom-modal").modal({ backdrop: 'static', keyboard: false, show: true });
                    $scope.CreandoComponentesModulos = false;
                }
                $("#mantipcom-modal").modal('hide');
            }

            $scope.ModificandoTipoComponente = function (tipocomponente) {
                $scope.formTipoComp.nombre = tipocomponente.Nombre;
                $scope.formTipoComp.extensiones = tipocomponente.Extensiones;
                $scope.formTipoComp.iscompbd = tipocomponente.isCompBD;
                $scope.formTipoComp.iscompdll = tipocomponente.isCompDLL;
                $scope.ModificarTipo = tipocomponente.idTipoComponentes;
            }

            $scope.CancelarModificacionTipoComponente = function () {
                $scope.formTipoComp.nombre = "";
                $scope.formTipoComp.extensiones = "";
                $scope.formTipoComp.iscompbd = "";
                $scope.formTipoComp.iscompdll = "";
                $scope.ModificarTipo = 0;
            }

            $scope.ModificarTipoComponente = function (formTipoComp) {
                if ($scope.lblModificarTipoComponente != "OK!") {
                    serviceModulos.updTipoComponente($scope.ModificarTipo, formTipoComp.nombre, formTipoComp.extensiones, formTipoComp.iscompbd, formTipoComp.iscompdll).success(function (data) {
                        $scope.lblModificarTipoComponente = "OK!";
                        $scope.CargarTipoComponentes();
                        $timeout(function () {
                            $scope.lblModificarTipoComponente = "Modificar";
                            $scope.CancelarModificacionTipoComponente();
                        }, 3000);
                    }).error(function (err) {
                        $scope.lblModificarTipoComponente = "!ERROR";
                    });
                }
            }

            $scope.LimpiarManComp = function () {
                $scope.formComp.nombre = "";
                $scope.formComp.tipocomponente = null;
                $scope.formComp.descripcion = "";
            }

            $scope.LimpiarManTipCom = function () {
                $scope.formTipoComp.nombre = "";
                $scope.formTipoComp.iscompbd = false;
            }

        }
    }
})();
