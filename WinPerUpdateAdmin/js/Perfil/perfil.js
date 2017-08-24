(function () {
    'use strict';

    angular
        .module('app')
        .controller('perfil', perfil);

    perfil.$inject = ['$scope','serviceSeguridad', '$window']; 

    function perfil($scope, serviceSeguridad, $window) {

        $scope.title = 'perfil';
        $scope.lblperfil = 'Modificar';
        $scope.inMdf = true;

        $scope.showAlerta = false;
        $scope.tipoAlerta = "alert-success";
        $scope.msgAlerta = "Clave cambiadas exitosamente!";

        activate();

        function activate() {
            $scope.msgError = "";
            $scope.id = $("#idToken").val();
            

            serviceSeguridad.getUsuario($scope.id).success(function (data) {
                $scope.user = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            $scope.ModificarPrf = function (valido) {
                if (valido) {
                    $scope.lblperfil = '';
                    $scope.inMdf = false;
                    serviceSeguridad.updUsuario($scope.id, $scope.user.CodPrf, $scope.user.Persona.Id, $scope.user.Persona.Apellidos, $scope.user.Persona.Nombres, $scope.user.Persona.Mail, $scope.user.EstUsr).success(function (data) {
                        $scope.showAlerta = true;
                        $scope.tipoAlerta = "alert-success";
                        $scope.msgAlerta = "Datos modificados con exito!";
                    }).error(function (err) {
                        $scope.showAlerta = true;
                        $scope.tipoAlerta = "alert-danger";
                        $scope.msgAlerta = "No se pudieron modificar los datos.";
                    });
                    $scope.lblperfil = 'Modificar';
                    $scope.inMdf = true;
                }
            }
        }
    }
})();
