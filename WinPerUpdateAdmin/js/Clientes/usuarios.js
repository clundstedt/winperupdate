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
            window.scrollTo(0, 0);
            $scope.emailFormat = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$/;
            $scope.msgError = "";
            $scope.msgSuccess = "";

            $scope.idUsuario = 0;
            $scope.Usuario = null;
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
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                    $scope.Usuario = data;
                    $scope.titulo = "Modificar Usuario";
                    $scope.labelcreate = "Modificar";
                    $scope.msgError = "";
                    console.log("estado = " + $scope.formData.estado);
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });

            }
            
            $scope.SaveUsuario = function (formData) {
                $scope.msgError = "";
                window.scrollTo(0, 0);
                if ($scope.idUsuario == 0) {
                    serviceClientes.getExisteMail(formData.mail).success(function (dataMail) {
                        if (!dataMail) {
                            $('#loading-modal').modal({ backdrop: 'static', keyboard: false });
                            $scope.lblStatUser += "Creando usuario...\n";
                            serviceClientes.addUsuario($scope.idCliente, formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                                $scope.idUsuario = data.Id;
                                $scope.usuarios = data;
                                $scope.formData.estado = "V",
                                $scope.titulo = "Modificar Usuario";
                                $scope.labelcreate = "Modificar";
                                $scope.lblStatUser += "Usuario creado con exito.\n";
                                if (formData.versionInicial) {
                                    $scope.lblStatUser += "Creando versión inicial del cliente...\n";
                                    serviceClientes.addVersionInicial('I', 'N', 'Versión inicial de WinPer', formData.hasDeploy).success(function (dataVI) {
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
                                                    $scope.lblStatUser += "Ocurrió un error durante el envío del correo de bienvenida, verifique consola del navegador.\n";
                                                    $timeout(function () {
                                                        $scope.AddUserOK = true;
                                                    }, 3000);
                                                });
                                            }).error(function (errCTV) {
                                                console.error(errCTV);
                                                $scope.lblStatUser += "Ocurrió un error durante la asignación de la versión al cliente, verifique consola del navegador.\n";
                                                $scope.AddUserOK = true;
                                            });
                                        }).error(function (err) {
                                            console.error(err);
                                            $scope.lblStatUser += "Ocurrió un error durante la generación del instalador de la versión inicial, verifique consola del navegador.\n";
                                            $scope.AddUserOK = true;
                                        });
                                    }).error(function (err) {
                                        console.error(err);
                                        $scope.lblStatUser += "Ocurrió un error durante la creación del a versión inicial, verifique consola del navegador.\n";
                                        $scope.AddUserOK = true;
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
                            }).error(function (err) {
                                console.error(err);
                                $scope.lblStatUser += "Ocurrió un error durante la creación, veirifique consola del navegador.\n";
                                $timeout(function () {
                                    $scope.AddUserOK = true;
                                }, 3000);
                            });
                        } else {
                            $scope.msgError = "El E-Mail indicado, ya existe!.";
                        }
                    }).error(function (err) {
                        console.error(err);
                        $scope.msgError = "Ocurrió un error, verifique consola del navegador";
                    });
                } else {
                    $scope.msgSuccess = "";
                    console.debug(formData.mail);
                    console.debug($scope.Usuario.Persona.Mail);
                    console.debug((formData.mail != $scope.Usuario.Persona.Mail));
                    if (formData.mail != $scope.Usuario.Persona.Mail) {
                        serviceClientes.getExisteMail(formData.mail).success(function (dataMail) {
                            if (!dataMail) {
                                serviceClientes.updUsuario($scope.idCliente, $scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
                                    $scope.titulo = "Modificar Usuario";
                                    $scope.labelcreate = "Modificar";
                                    $scope.msgSuccess = "Usuario modificado exitosamente!.";
                                    $scope.msgError = "";
                                    $scope.Usuario.Persona.Mail = formData.mail;
                                }).error(function (err) {
                                    console.error(err);
                                    $scope.msgError = "Ocurrió un error, verifique consola del navegador";
                                });
                            } else {
                                $scope.msgError = "El E-Mail indicado, ya existe!.";
                            }
                        }).error(function (err) {
                            console.error(err);
                            $scope.msgError = "Ocurrió un error, verifique consola del navegador";
                        })
                    } else {
                        serviceClientes.updUsuario($scope.idCliente, $scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
                            $scope.titulo = "Modificar Usuario";
                            $scope.labelcreate = "Modificar";
                            $scope.msgSuccess = "Usuario modificado exitosamente!.";
                            $scope.msgError = "";
                            $scope.Usuario.Persona.Mail = formData.mail;
                        }).error(function (err) {
                            console.error(err);
                            $scope.msgError = "Ocurrió un error, verifique consola del navegador";
                        });
                    }
                }
            }

            $scope.ReenviarMailBienvenida = function () {
                $scope.msgSuccess = "";
                $scope.msgError = "";
                $('#loading-modal').modal({ backdrop: 'static', keyboard: false });
                $scope.AddUserOK = false;
                $scope.lblStatUser = "Reenviando E-Mail de bienvenida...\n";
                serviceClientes.EnviarBienvenida($scope.idUsuario).success(function (dataEB) {
                    if (dataEB.CodErr == 0) {
                        $scope.msgSuccess = "Se ha enviado el E-Mail de bienvenida correctamente\n";
                    } else if (dataEB.CodErr == 1) {
                        $scope.msgError = "No se pudo enviar el e-mail de bienvenida, verifique que los datos esten bien escritos.\n";
                    } else if (dataEB.CodErr == 2) {
                        $scope.msgError = "No existe el usuario.\n";
                    } else if (dataEB.CodErr == 3) {
                        $scope.msgError = "No existe el cliente del usuario.\n";
                    }
                    $('#loading-modal').modal('toggle');
                }).error(function (errEB) {
                    console.error(errEB);
                    $scope.msgError = "Ocurrió un error durante el envio del correo de bienvenida, verifique consola del navegador.\n";
                    $('#loading-modal').modal('toggle');
                    $scope.AddUserOK = true;
                });
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.Eliminar = function (estado) {
                $scope.msgSuccess = "";
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
                                                    $scope.formData.estado = estado;
                                                    $scope.msgError = "";
                                                    $scope.msgSuccess = "Cambios realizados exitosamente!.";
                                                }).error(function (err) {
                                                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                                                });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            $scope.CerrarLoadingModal = function () {
                $("#loading-modal").modal('toggle');
            }

        }
    }
})();
