(function () {
    'use strict';

    angular
        .module('app')
        .controller('controllerHome', controllerHome);

    controllerHome.$inject = ['$scope','factoryHome']; 

    function controllerHome($scope, factoryHome) {
        $scope.title = 'controllerHome';
        $scope.id = $("#idToken").val();
        $scope.formData = {};
        $scope.showAlerta = false;
        $scope.blockBoton = false;
        $scope.tipoAlerta = "alert-success";
        $scope.msgAlerta = "Clave cambiadas exitosamente!";

        
        
        activate();

        function activate() {


            factoryHome.getUsuarioSession($scope.id).success(function (data) {
                $scope.userSession = data;
            }).error(function (data) {
                console.error(data);
            });

            $scope.VerificarPwdIguales = function (formData) {
                return !(formData.pwdNueva == formData.pwdNuevaR);
            }

            $scope.cancelarCambioPwd = function (formDataU) {
                formDataU.pwdActual = "";
                formDataU.pwdNueva = "";
                formDataU.pwdNuevaR = "";
                $scope.showAlerta = false;

            }

            $scope.updPwd = function (formDataU) {

                factoryHome.compruebaPwd($scope.userSession.Persona.Mail, formDataU.pwdActual, formDataU.pwdNueva).success(function (data) {
                    $scope.showAlerta = true;
                    $scope.tipoAlerta = "alert-success";
                    $scope.msgAlerta = "Clave cambiada exitosamente!";
                    $scope.blockBoton = true;
                    setTimeout(function () {
                        $("#modalCambioPwd").modal('toggle');
                        formDataU.pwdActual = "";
                        formDataU.pwdNueva = "";
                        formDataU.pwdNuevaR = "";
                        $scope.showAlerta = false;
                        $scope.blockBoton = false;
                    }, 3000);
                }).error(function (err) {
                    $scope.showAlerta = true;
                    $scope.tipoAlerta = "alert-danger";
                    $scope.msgAlerta = "No se pudo cambiar la clave!";
                });
            }
        }
    }
})();
