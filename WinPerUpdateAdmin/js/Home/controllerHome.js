(function () {
    'use strict';

    angular
        .module('app')
        .controller('controllerHome', controllerHome);

    controllerHome.$inject = ['$scope', 'factoryHome', '$timeout', '$window'];

    function controllerHome($scope, factoryHome, $timeout, $window) {
        $scope.msgError = "";

        $scope.title = 'controllerHome';
        $scope.id = $("#idToken").val();
        $scope.formData = {};
        $scope.showAlerta = false;
        $scope.blockBoton = false;
        $scope.tipoAlerta = "alert-success";
        $scope.msgAlerta = "Clave cambiadas exitosamente!";
        
        
        activate();

        function activate() {

            /*
            $timeout(function () {
                factoryHome.verificaSession().success(function (data) {
                    if(data)$window.location.href = '/Home';
                }).error(function (err) {
                    console.error(err);
                });
            }, 60000);*/

            factoryHome.getUsuarioSession($scope.id).success(function (data) {
                $scope.userSession = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
