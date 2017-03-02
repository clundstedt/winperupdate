(function () {
    'use strict';

    angular
        .module('app')
        .controller('usuarios', usuarios);

    usuarios.$inject = ['$scope', '$window', '$routeParams', 'serviceClientes', 'serviceSeguridad', '$timeout'];

    function usuarios($scope, $window, $routeParams, serviceClientes, serviceSeguridad, $timeout) {
        $scope.title = 'usuarios';

        activate();

        function activate() {
            $scope.idUsuario = 0;
            $scope.titulo = "Crear Usuario";
            $scope.labelcreate = "Crear un Usuario";
            $scope.increate = true;
            $scope.formData = {};
            $scope.usuarios = [];
            $scope.mensaje = '';
            $scope.perfiles = [{ CodPrf: 11, NomPrf: 'Administrador' },
                               { CodPrf: 12, NomPrf: 'DBA' }];

            $scope.idCliente = $routeParams.idCliente;

            $scope.lblStatUser = "";
            $scope.AddUserOK = false;

            serviceClientes.getCliente($scope.idCliente).success(function (data) {
                $scope.cliente = data;
            }).error(function (data) {
                console.error(data);
            });

            if ($routeParams.idUsuario > 0) {
                $scope.idUsuario = $routeParams.idUsuario;

                serviceClientes.getUsuario($scope.idCliente, $scope.idUsuario).success(function (data) {
                    $scope.formData.perfil = data.CodPrf;
                    $scope.formData.apellido = data.Persona.Apellidos;
                    $scope.formData.nombre = data.Persona.Nombres;
                    $scope.formData.mail = data.Persona.Mail;
                    $scope.formData.idPersona = data.Persona.Id;
                    $scope.formData.estado = data.EstUsr,
                    $scope.titulo = "Modificar Usuario";
                    $scope.labelcreate = "Modificar";

                    console.log("estado = " + $scope.formData.estado);
                }).error(function (err) {
                    console.log(err);
                });

            }
            
            $scope.SaveUsuario = function (formData) {
                $('#loading-modal').modal({ backdrop: 'static', keyboard: false })
                if ($scope.idUsuario == 0) {
                    $scope.lblStatUser += "Creando usuario...\n";
                    serviceClientes.addUsuario($scope.idCliente, formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                        $scope.idUsuario = data.Id;
                        $scope.formData.estado = "V",
                        $scope.titulo = "Modificar Usuario";
                        $scope.labelcreate = "Modificar";
                        $scope.lblStatUser += "Usuario creado con exito. Verificando versión inicial del cliente...\n";
                        serviceClientes.ExisteVersionInicial($scope.idCliente).success(function (dataVer) {
                            if (!dataVer) {
                                $scope.lblStatUser += "Creando versión inicial del cliente...\n";
                                serviceClientes.addVersionInicial('I', 'N', 'Versión inicial de WinPer').success(function (dataVI) {
                                    $scope.lblStatUser += "Generando instalador de la versión inicial...\n";
                                    serviceClientes.GenVersionInicial(dataVI.IdVersion).success(function (data1) {
                                        $scope.lblStatUser += "Asignando versión al cliente...\n";
                                        serviceClientes.addClienteToVersion(data1.IdVersion, $scope.idCliente).success(function (dataCTV) {
                                            $scope.lblStatUser += "Versión inicial creada y publicada correctamente. Enviando E-Mail de bienvenida....\n";
                                            serviceClientes.EnviarBienvenida(data.Id).success(function (dataEB) {
                                                if (dataEB.CodErr == 0) {
                                                    $scope.lblStatUser += "Se ha enviado el E-Mail de bienvenida correctamente\n";
                                                } else if (dataEB.CodErr == 1) {
                                                    $scope.lblStatUser += "No se pudo enviar el e-mail de bienvenida, verifique que los datos esten bien escritos.\n";
                                                } else if (dataEB.CodErr == 2) {
                                                    $scope.lblStatUser += "No existe el usuario.\n";
                                                } else if (dataEB.CodErr == 3) {
                                                    $scope.lblStatUser += "No existe el cliente del usuario.\n";
                                                }
                                                $timeout(function () {
                                                    $scope.AddUserOK = true;
                                                }, 3000);
                                            }).error(function (errEB) {
                                                console.error(errEB);
                                                $scope.lblStatUser += "Ocurrió un error durante el envio del correo de bienvenida, verifique consola del navegador.\n";
                                                $timeout(function () {
                                                    $scope.AddUserOK = true;
                                                }, 3000);
                                            });
                                        }).error(function (errCTV) {
                                            console.error(errCTV);
                                            $scope.lblStatUser += "Ocurrió un error durante la asignacion de la version al cliente, verifique consola del navegador.\n";
                                        });
                                    }).error(function (err) {
                                        console.error(err);
                                        $scope.lblStatUser += "Ocurrió un error durante la generacion del instalador de la version inicial, verifique consola del navegador.\n";
                                    });
                                }).error(function (err) {
                                    console.error(err);
                                    $scope.lblStatUser += "Ocurrió un error durante la creacion del a version inicial, verifique consola del navegador.\n";
                                });
                            } else {
                                serviceClientes.EnviarBienvenida(data.Id).success(function (dataEB) {
                                    if (dataEB.CodErr == 0) {
                                        $scope.lblStatUser += "Se ha enviado el E-Mail de bienvenida correctamente\n";
                                    } else if (dataEB.CodErr == 1) {
                                        $scope.lblStatUser += "No se pudo enviar el e-mail de bienvenida, verifique que los datos esten bien escritos.\n";
                                    } else if (dataEB.CodErr == 2) {
                                        $scope.lblStatUser += "No existe el usuario.\n";
                                    } else if (dataEB.CodErr == 3) {
                                        $scope.lblStatUser += "No existe el cliente del usuario.\n";
                                    }
                                    $timeout(function () {
                                        $scope.AddUserOK = true;
                                    }, 3000);
                                }).error(function (errEB) {
                                    console.error(errEB);
                                    $scope.lblStatUser += "Ocurrió un error durante el envio del correo de bienvenida, verifique consola del navegador.\n";
                                    $timeout(function () {
                                        $scope.AddUserOK = true;
                                    }, 3000);
                                });
                            }
                        }).error(function (errorVer) {
                            console.error(errorVer);
                            $scope.lblStatUser += "Ocurrió un error durante la verificacion de la version inicial, verifique consola del navegador.\n";
                            $timeout(function () {
                                $scope.AddUserOK = true;
                            }, 3000);
                        });
                    }).error(function (err) {
                        console.error(err);
                        $scope.lblStatUser += "Ocurrió un error durante la creación, veirifique consola del navegador.\n";
                        $timeout(function () {
                            $scope.AddUserOK = true;
                        },3000);
                    });
                }
                else {
                    $scope.lblStatUser += "Modificando usuario...\n";
                    serviceClientes.updUsuario($scope.idCliente, $scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
                        $scope.titulo = "Modificar Usuario";
                        $scope.labelcreate = "Modificar";
                        $scope.lblStatUser += "Usuario Modificado\n";
                        $timeout(function () {
                            $scope.AddUserOK = true;
                        }, 3000);
                    }).error(function (err) {
                        console.error(err);
                        $scope.lblStatUser += "Ocurrió un error, verifique consola del navegador\n";
                        $timeout(function () {
                            $scope.AddUserOK = true;
                        }, 3000);
                    });
                }
            }

            $scope.ReenviarMailBienvenida = function () {
                $('#loading-modal').modal({ backdrop: 'static', keyboard: false })
                $scope.AddUserOK = false;
                $scope.lblStatUser = "Reenviando E-Mail de bienvenida...\n";
                serviceClientes.EnviarBienvenida($scope.idUsuario).success(function (dataEB) {
                    if (dataEB.CodErr == 0) {
                        $scope.lblStatUser += "Se ha enviado el E-Mail de bienvenida correctamente\n";
                    } else if (dataEB.CodErr == 1) {
                        $scope.lblStatUser += "No se pudo enviar el e-mail de bienvenida, verifique que los datos esten bien escritos.\n";
                    } else if (dataEB.CodErr == 2) {
                        $scope.lblStatUser += "No existe el usuario.\n";
                    } else if (dataEB.CodErr == 3) {
                        $scope.lblStatUser += "No existe el cliente del usuario.\n";
                    }
                    $timeout(function () {
                        $scope.AddUserOK = true;
                    }, 3000);
                }).error(function (errEB) {
                    console.error(errEB);
                    $scope.lblStatUser += "Ocurrió un error durante el envio del correo de bienvenida, verifique consola del navegador.\n";
                    $timeout(function () {
                        $scope.AddUserOK = true;
                    }, 3000);
                });
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.Eliminar = function (estado) {
                serviceClientes.getUsuario($scope.idCliente, $scope.idUsuario).success(function (data) {
                    serviceClientes.updUsuario( $scope.idCliente,
                                                $scope.idUsuario,
                                                data.CodPrf,
                                                data.Persona.Id,
                                                data.Persona.Apellidos,
                                                data.Persona.Nombres,
                                                data.Persona.Mail,
                                                estado).success(function (data2) {
                                                    $('.close').click();

                                                    $window.setTimeout(function () {
                                                        $window.location.href = "/Clientes#/EditCliente/" + $scope.idCliente;
                                                    }, 2000);
                                                }).error(function (err) {
                                                    console.log(err);
                                                });
                }).error(function (data) {
                    console.error(data);
                });
            }

            $scope.CerrarLoadingModal = function () {
                $("#loading-modal").modal('toggle');
            }

        }
    }
})();
