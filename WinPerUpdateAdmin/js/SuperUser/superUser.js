(function () {
    'use strict';

    angular
        .module('app')
        .controller('superUser', superUser);

    superUser.$inject = ['$scope', '$routeParams', '$window', 'serviceSU', '$timeout'];

    function superUser($scope, $routeParams, $window, serviceSU, $timeout) {
        $scope.title = 'Configuración General';

        activate();

        function activate() {
            $scope.msgError = "";
            
            $scope.lblButton = "Guardar";
            $scope.formData = {};

            serviceSU.LoadWebConf().success(function (data) {
                $scope.formData.innosetup = data.pathGenSetup;
                $scope.formData.smtpwu = data.hostMail;
                $scope.formData.mailwu = data.userMail;
                $scope.formData.passmailwu = data.pwdMail;
                $scope.formData.rpassmailwu = data.pwdMail;
                $scope.formData.aliasmailwu = data.fromMail;
                $scope.formData.mailsoporte = data.correoSoporte;
                $scope.formData.dirupload = data.upload;
                $scope.formData.dirvoficial = data.voficial;
                $scope.formData.dirfuentes = data.fuentes;
                $scope.formData.portmail = data.portMail;
                $scope.formData.sslmail = data.sslMail;

                console.log($scope.formData.sslmail);
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            $scope.ConfirmarSave = function () {
                $("#confirmsave-modal").modal('show');
            }

            $scope.GuardarConf = function (formData) {
                $scope.lblButton = "Guardando";
                serviceSU.Guardar(formData.innosetup, formData.smtpwu, formData.mailwu, formData.passmailwu, formData.aliasmailwu, formData.mailsoporte, formData.dirupload, formData.dirvoficial, formData.dirfuentes, formData.portmail, formData.sslmail).success(function (data) {
                    serviceSU.LoadWebConf().success(function (dataLoad) {
                        $scope.formData.innosetup = dataLoad.pathGenSetup;
                        $scope.formData.smtpwu = dataLoad.hostMail;
                        $scope.formData.mailwu = dataLoad.userMail;
                        $scope.formData.passmailwu = dataLoad.pwdMail;
                        $scope.formData.rpassmailwu = dataLoad.pwdMail;
                        $scope.formData.aliasmailwu = dataLoad.fromMail;
                        $scope.formData.mailsoporte = dataLoad.correoSoporte;
                        $scope.formData.dirupload = dataLoad.upload;
                        $scope.formData.dirvoficial = dataLoad.voficial;
                        $scope.formData.dirfuentes = dataLoad.fuentes;
                        $scope.formData.portmail = dataLoad.portMail;
                        $scope.formData.sslmail = dataLoad.sslMail;
                        $scope.lblButton = "Guardar";
                        $timeout(function () {
                            $window.location.href = "/Home/Logout/";
                        }, 1500);
                        $scope.msgError = "";
                    }).error(function (err) {
                        console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                        $scope.lblButton = "Guardar";
                    });
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    $scope.lblButton = "Guardar";
                });
            }
        }
    }
})();
