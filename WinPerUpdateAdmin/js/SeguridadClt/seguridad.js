(function () {
    'use strict';

    angular
        .module('app')
        .controller('seguridad', seguridad);

    seguridad.$inject = ['$scope', '$routeParams', '$window', 'serviceSeguridad', 'serviceClientes'];

    function seguridad($scope, $routeParams, $window, serviceSeguridad, serviceClientes) {
        $scope.title = 'Seguridad';

        activate();

        function activate() {
            console.log("id User = " + $("#idToken").val());

            $scope.idCliente = $routeParams.idCliente > 0 ? $routeParams.idCliente : 0;
            $scope.idUsuario = 0;
            $scope.titulo = "Crear Usuario";
            $scope.labelcreate = "Crear un Usuario";
            $scope.increate = true;
            $scope.formData = {};
            $scope.idUsuario = 0;
            $scope.usuarios = [];
            $scope.totales = [0, 0];
            $scope.perfiles = [{ CodPrf: 11, NomPrf: 'Administrador' },
                               { CodPrf: 12, NomPrf: 'DBA' }];

            serviceSeguridad.getUsuario($("#idToken").val()).success(function (data) {
                $scope.idCliente = data.Cliente.Id;

                serviceClientes.getUsuarios($scope.idCliente).success(function (usuarios) {
                    $scope.usuarios = usuarios;

                    angular.forEach($scope.usuarios, function (item) {
                        $scope.totales[item.CodPrf - 11]++;
                    });

                }).error(function (err) {
                    console.error(err);
                });

            }).error(function (err) {
                console.error(err);
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

                    console.log("formData.estado = " + $scope.formData.estado);
                }).error(function (err) {
                    console.log(err);
                });
            }

            $scope.SaveUsuario = function (formData) {
                console.debug("formData = " + JSON.stringify(formData));
                if ($scope.idUsuario == 0) {
                    serviceClientes.addUsuario($scope.idCliente, formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                        $scope.idUsuario = data.Id;
                        $scope.titulo = "Modificar Usuario";
                        $scope.labelcreate = "Modificar";
                    }).error(function (err) {
                        console.log(err);
                    });
                }
                else {
                    serviceClientes.updUsuario($scope.idCliente, $scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
                        $scope.titulo = "Modificar Usuario";
                        $scope.labelcreate = "Modificar";
                    }).error(function (err) {
                        console.log(err);
                    });
                }
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.Eliminar = function (estado) {
                serviceClientes.getUsuario($scope.idCliente, $scope.idUsuario).success(function (data) {
                    serviceClientes.updUsuario($scope.idCliente,
                                                $scope.idUsuario,
                                                data.CodPrf,
                                                data.Persona.Id,
                                                data.Persona.Apellidos,
                                                data.Persona.Nombres,
                                                data.Persona.Mail,
                                                estado).success(function (data2) {
                                                    $('.close').click();

                                                    $window.setTimeout(function () {
                                                        $window.location.href = "/SeguridadClt#/Usuarios/";
                                                    }, 2000);
                                                }).error(function (err) {
                                                    console.log(err);
                                                });
                }).error(function (data) {
                    console.error(data);
                });
            }

        }
    }
})();
