(function () {
    'use strict';

    angular
        .module('app')
        .controller('mantenedor', mantenedor);

    mantenedor.$inject = ['$scope', '$window', '$routeParams', 'serviceSeguridad'];

    function mantenedor($scope, $window, $routeParams, serviceSeguridad) {
        $scope.title = 'mantenedor';

        activate();

        function activate() {
            $scope.emailFormat = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$/;

            $scope.msgError = "";
            $scope.saveUser = "";
            $scope.msgCamposRequeridos = false;

            $scope.idUsuario = 0;
            $scope.titulo = "Crear Usuario";
            $scope.labelcreate = "Crear un Usuario";
            $scope.increate = true;
            $scope.formData = {};
            $scope.mensaje = '';
            $scope.perfiles = [];

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idUsuario = $routeParams.idUsuario;
                $scope.titulo = "Modificar Usuario";
                $scope.labelcreate = "Modificar";

                serviceSeguridad.getUsuario($scope.idUsuario).success(function (data) {
                    $scope.formData.nombre = data.Persona.Nombres;
                    $scope.formData.apellido = data.Persona.Apellidos;
                    $scope.formData.mail = data.Persona.Mail;
                    $scope.formData.perfil = data.CodPrf;
                    $scope.formData.idPersona = data.Persona.Id;
                    $scope.formData.estado = data.EstUsr;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

            serviceSeguridad.getPerfiles().success(function (data) {
                $scope.perfiles = data;
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
            });

            $scope.SaveUsuario = function (formData, frmValid) {
                $scope.saveUser = "";
                $scope.msgCamposRequeridos = !frmValid;
                if (frmValid) {
                    if ($scope.idUsuario == 0) {
                        serviceSeguridad.addUsuario(formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                            $scope.idUsuario = data.Id;
                            $scope.titulo = "Modificar Usuario";
                            $scope.labelcreate = "Modificar";
                            $scope.msgError = "";
                            $scope.formData.nombre = data.Persona.Nombres;
                            $scope.formData.apellido = data.Persona.Apellidos;
                            $scope.formData.mail = data.Persona.Mail;
                            $scope.formData.perfil = data.CodPrf;
                            $scope.formData.idPersona = data.Persona.Id;
                            $scope.formData.estado = data.EstUsr;
                            $scope.saveUser = "Usuario creado exitosamente!.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    }
                    else {
                        serviceSeguridad.updUsuario($scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
                            $scope.titulo = "Modificar Usuario";
                            $scope.labelcreate = "Modificar";
                            $scope.msgError = "";
                            $scope.formData.nombre = data.Persona.Nombres;
                            $scope.formData.apellido = data.Persona.Apellidos;
                            $scope.formData.mail = data.Persona.Mail;
                            $scope.formData.perfil = data.CodPrf;
                            $scope.formData.idPersona = data.Persona.Id;
                            $scope.formData.estado = data.EstUsr;
                            $scope.saveUser = "Usuario modificado exitosamente!.";
                        }).error(function (err) {
                            console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        });
                    }
                } 
                
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.Eliminar = function (estado) {
                $scope.saveUser = "";
                serviceSeguridad.getUsuario($scope.idUsuario).success(function (data) {
                    serviceSeguridad.updUsuario($scope.idUsuario,
                                                data.CodPrf,
                                                data.Persona.Id,
                                                data.Persona.Apellidos,
                                                data.Persona.Nombres,
                                                data.Persona.Mail,
                                                estado).success(function (data2) {

                        $('.close').click();

                        $scope.formData.estado = estado;
                        $scope.msgError = "";
                        $scope.saveUser = "Cambios realizados exitosamente!.";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

        }
    }
})();
