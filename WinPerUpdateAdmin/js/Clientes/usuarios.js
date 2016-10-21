(function () {
    'use strict';

    angular
        .module('app')
        .controller('usuarios', usuarios);

    usuarios.$inject = ['$scope', '$window', '$routeParams', 'serviceClientes', 'serviceSeguridad'];

    function usuarios($scope, $window, $routeParams, serviceClientes, serviceSeguridad) {
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
                console.debug("formData = " + JSON.stringify(formData));
                if ($scope.idUsuario == 0) {
                    serviceClientes.addUsuario($scope.idCliente, formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                        $scope.idUsuario = data.Id;
                        $scope.formData.estado = "V",
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


        }
    }
})();
