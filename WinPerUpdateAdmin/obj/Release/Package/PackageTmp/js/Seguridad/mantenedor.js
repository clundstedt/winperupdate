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
            $scope.idUsuario = 0;
            $scope.titulo = "Crear Usuario";
            $scope.labelcreate = "Crear un Usuario";
            $scope.increate = true;
            $scope.formData = {};
            $scope.mensaje = '';
            $scope.perfiles = [{ CodPrf: 1, NomPrf: 'Administrador' },
                               { CodPrf: 2, NomPrf: 'Desarrollador' },
                               { CodPrf: 3, NomPrf: 'Soporte' },
                               { CodPrf: 4, NomPrf: 'Gestión' }];

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

                }).error(function (data) {
                    console.error(data);
                });
            }

            $scope.SaveUsuario = function (formData) {
                console.debug("formData = " + JSON.stringify(formData));
                if ($scope.idUsuario == 0) {
                    serviceSeguridad.addUsuario(formData.perfil, formData.apellido, formData.nombre, formData.mail, 'V').success(function (data) {
                        $scope.idUsuario = data.Id;
                        $scope.titulo = "Modificar Usuario";
                        $scope.labelcreate = "Modificar";
                    }).error(function (err) {
                        console.log(err);
                    });
                }
                else {
                    serviceSeguridad.updUsuario($scope.idUsuario, formData.perfil, formData.idPersona, formData.apellido, formData.nombre, formData.mail, formData.estado).success(function (data) {
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

            $scope.Eliminar = function () {
                serviceSeguridad.getUsuario($scope.idUsuario).success(function (data) {
                    serviceSeguridad.updUsuario($scope.idUsuario,
                                                data.CodPrf,
                                                data.Persona.Id,
                                                data.Persona.Apellidos,
                                                data.Persona.Nombres,
                                                data.Persona.Mail,
                                                'C').success(function (data2) {
                        $('.close').click();

                        $window.setTimeout(function () {
                            $window.location.href = "/Seguridad#/";
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
