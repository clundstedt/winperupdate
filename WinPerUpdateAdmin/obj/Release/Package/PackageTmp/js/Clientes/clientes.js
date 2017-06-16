﻿/// <reference path="serviceClientes.js" />
/// <reference path="serviceClientes.js" />
(function () {
    'use strict';

    angular
        .module('app')
        .controller('clientes', clientes);

    clientes.$inject = ['$scope', '$window', '$routeParams', 'serviceClientes'];

    function clientes($scope, $window, $routeParams, serviceClientes) {
        $scope.title = 'clientes';
        $scope.regiones = [];
        $scope.comunas = [];

        activate();

        function activate() {
            $scope.msgError = "";
            $scope.msgSuccess = "";

            $scope.modulosWinper = [];
            $scope.idCliente = 0;
            $scope.titulo = "Crear Cliente";
            $scope.labelcreate = "Crear un Cliente";
            $scope.increate = true;
            $scope.rutok = true;
            $scope.formData = {};
            $scope.formData.modulos = [];
            $scope.mensaje = '';
            $scope.totales = [0, 0];
            $scope.usuarios = [];
            $scope.estadosMantencion = [{ valor: 7, nomest: "Activo" }, { valor: 9, nomest: "No Activo" }];
            $scope.mesesInicioMantencion = [
            { valor: "01", nommes: "Enero" },
            { valor: "02", nommes: "Febrero" },
            { valor: "03", nommes: "Marzo" },
            { valor: "04", nommes: "Abril" },
            { valor: "05", nommes: "Mayo" },
            { valor: "06", nommes: "Junio" },
            { valor: "07", nommes: "Julio" },
            { valor: "08", nommes: "Agosto" },
            { valor: "09", nommes: "Septiembre" },
            { valor: "10", nommes: "Octubre" },
            { valor: "11", nommes: "Noviembre" },
            { valor: "12", nommes: "Diciembre" }, ];

            $scope.aniosInicioContrato = [];


            $scope.versionesCliente = [];

            $scope.trabPlantas = [];
            $scope.trabHonorarios = [];

            $scope.suites = [];

            serviceClientes.getAnios().success(function (data) {
                $scope.aniosInicioContrato = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });
            
            serviceClientes.getRegiones().success(function (regiones) {
                $scope.regiones = regiones;
                $scope.msgError = "";
            }).error(function (error) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            $scope.GenerarPDFCliente = function (idCliente) {
                $window.location.href = '/api/Clientes/'+idCliente+'/PDF';
            }

            $scope.GenCantUserPerm = function (data) {
                while (!angular.isUndefined(data) && data.length < 4) {
                    data = "0" + data;
                }
                return data;
            }


            //se debe llamar una vez que se seleccione una suite y hay que hacer la busqueda por de modulo by suite
            $scope.CargarModulosBySuite = function (formData) {
                serviceClientes.getModulosBySuite(formData.suite).success(function (modulos) {
                    $scope.modulosWinper = modulos;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }
            
            $scope.SeleccionarTodosModulos = function () {
                for (var i = 0; i < $scope.modulosWinper.length; i++) {
                    if (!$scope.isSelected($scope.modulosWinper[i].idModulo)) {
                        $scope.formData.modulos.push($scope.modulosWinper[i]);
                    }
                }
                console.log($scope.modulosWinper);
            }

            $scope.LimpiarSeleccionados = function (formData) {
                $scope.tmpModulos = [];
                for (var i = 0; i < $scope.formData.modulos.length; i++) {
                    if($scope.formData.modulos[i].Suite != formData.suite){
                        $scope.tmpModulos.push($scope.formData.modulos[i]);
                    }
                }
                $scope.formData.modulos = $scope.tmpModulos;
               
            }

            $scope.isSelected = function (idModulo) {
                for (var i = 0; i < $scope.formData.modulos.length; i++) {
                    if ($scope.formData.modulos[i].idModulo == idModulo) {
                        return true;
                    }
                }
                return false;
            }

            serviceClientes.getTrabPlantas().success(function (data) {
                $scope.trabPlantas = data;
            }).error(function (err) {
                console.error(err)
            });

            serviceClientes.getTrabHonorarios().success(function (data) {
                $scope.trabHonorarios = data;
            }).error(function (err) {
                console.error(err)
            });

            serviceClientes.getSuites().success(function (data) {
                $scope.suites = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });
            

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idCliente = $routeParams.idCliente;
                $scope.titulo = "Modificar Cliente";
                $scope.labelcreate = "Modificar";

                serviceClientes.getCliente($scope.idCliente).success(function (data) {
                    $scope.formData.rut = data.RutFmt;
                    $scope.formData.nombre = data.Nombre;
                    $scope.formData.direccion = data.Direccion;
                    $scope.formData.region = data.Comuna.Region.idRgn;
                    $scope.formData.licencia = data.NroLicencia;
                    $scope.formData.folio = data.NumFolio;
                    $scope.formData.mescon = data.MesCon;
                    $scope.formData.estmtc = data.EstMtc;
                    $scope.formData.mesini = data.Mesini;
                    $scope.formData.nrotrbc = data.NroTrbc;
                    $scope.formData.nrotrbh = data.NroTrbh;
                    $scope.formData.nrousr = data.NroUsr;
                    $scope.formData.correlativo = data.Correlativo;
                    $scope.formData.estado = data.Estado;

                    $scope.CargarModulosCliente($scope.idCliente);

                    serviceClientes.getComunas(data.Comuna.Region.idRgn).success(function (data2) {
                        $scope.comunas = data2;

                        $scope.formData.comuna = data.Comuna.idCmn;
                    });

                    serviceClientes.getUsuarios($scope.idCliente).success(function (data) {
                        $scope.usuarios = data;

                        angular.forEach($scope.usuarios, function (item) {
                            $scope.totales[item.CodPrf - 11]++;
                        });

                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });

                    serviceClientes.getVersionesClientes($scope.idCliente).success(function (data) {
                        $scope.versionesCliente = data;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });


            } 

            $scope.Comunas = function (formData) {
                serviceClientes.getComunas(formData.region).success(function (data) {
                    $scope.comunas = data;
                });
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.ValidarRut = function (formData) {
                var _value = formData.rut;
                if (_value.length < 2) return;

                _value = _value.replace(/[^0-9kK]+/g, '').toUpperCase();

                var result = _value.slice(-4, -1) + '-' + _value.substr(_value.length - 1);
                for (var i = 4; i < _value.length; i += 3) result = _value.slice(-3 - i, -i) + '' + result;

                formData.rut = result;

                var t = parseInt(_value.slice(0, -1), 10), m = 0, s = 1;
                while (t > 0) {
                    s = (s + t % 10 * (9 - m++ % 6)) % 11;
                    t = Math.floor(t / 10);
                }
                var v = (s > 0) ? (s - 1) + '' : 'K';
                $scope.rutok = (v === _value.slice(-1));
            }

            $scope.SaveCliente = function (formData) {
                console.log(JSON.stringify(formData));
                var arrRut = formData.rut.split('-');
                $scope.increate = false;
                $scope.labelcreate = "Enviando";
                $scope.msgSuccess = "";

                if ($scope.idCliente == 0) {
                    serviceClientes.addCliente(arrRut[0], arrRut[1], formData.nombre, formData.direccion, formData.comuna, formData.licencia, formData.folio, formData.estmtc, formData.mesini, formData.nrotrbc, formData.nrotrbh, formData.nrousr, formData.mescon, formData.correlativo).success(function (data) {
                        $scope.increate = true;
                        $scope.labelcreate = "Modificar";
                        $scope.idCliente = data.Id;
                        serviceClientes.addModuloCliente(data.Id, $scope.formData.modulos).success(function (data) {
                            $scope.CargarModulosCliente(data.Id);
                            $scope.msgError = "";
                            $scope.msgSuccess = "Cliente creado exitosamente!.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                        });
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
                else {
                    serviceClientes.updCliente($scope.idCliente, arrRut[0], arrRut[1], formData.nombre, formData.direccion, formData.comuna, formData.licencia, formData.folio, formData.estmtc, formData.mesini, formData.nrotrbc, formData.nrotrbh, formData.nrousr, formData.mescon, formData.mescon, formData.correlativo).success(function (data) {
                        $scope.increate = true;
                        $scope.labelcreate = "Modificar";
                        serviceClientes.addModuloCliente($scope.idCliente, $scope.formData.modulos).success(function (data) {
                            $scope.CargarModulosCliente($scope.idCliente);
                            $scope.msgError = "";
                            $scope.msgSuccess = "Cliente modificado exitosamente!.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                        });
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
            }

            $scope.CargarModulosCliente = function (idCliente) {
                serviceClientes.getModulosCliente(idCliente).success(function (data) {
                    $scope.formData.modulos = data;
                    $scope.modulosWinper = $scope.formData.modulos;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.Eliminar = function (est) {
                $scope.msgSuccess = "";
                serviceClientes.delCliente($scope.idCliente, est).success(function (data) {
                    $scope.formData.estado = est;
                    $scope.msgError = "";
                    $scope.msgSuccess = "Cambio realizado exitosamente!.";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.GenKey = function (formOK, formData) {
                if (formOK) {
                    serviceClientes.getKey(formData.folio.toString().charAt(3), formData.mescon, formData.correlativo, formData.estmtc, formData.mesini, formData.nrotrbc, formData.nrotrbh, formData.nrousr).success(function (data) {
                        formData.licencia = data;
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                } else {
                    $("#genkey-modal").modal('show');
                }
            }

            $scope.GenCorrelativo = function () {
                if (!(typeof $scope.formData.mescon === "undefined") && !(typeof $scope.formData.folio === "undefined"))
                {
                    serviceClientes.GenCorrelativo($scope.formData.folio, $scope.formData.mescon).success(function (data) {
                        $scope.formData.correlativo = data;
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                    });
                }
                else
                {
                    $scope.formData.correlativo = null;
                }
            }


        }

    }
})();
