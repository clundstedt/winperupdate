(function () {
    'use strict';

    angular
        .module('app')
        .controller('login', login);

    login.$inject = ['$scope','$window', 'serviceLogin', '$timeout']; 

    function login($scope, $window, serviceLogin, $timeout) {
        $scope.title = 'login';
        $scope.formData = {};

        activate();

        function activate() {
            $scope.inlogin = true;
            $scope.inenvioclave = true;
            $scope.labellogin = "Ingresar";
            $scope.labelloginsendclave = "Enviar Clave";
            $scope.msgerror = "";
            $scope.lblButton = "Instalar";
            $scope.lblLoad = "";

            $scope.KeyUpEvent = function (KeyCode, formData) {
                if (KeyCode == 13) {
                    $scope.CheckAccess(formData);
                }
            }

            $scope.CheckAccess = function (formData) {
                $scope.inlogin = false;
                $scope.labellogin = "Validando Ingreso";
                $scope.errorlogin = false;
                $scope.msgerror = '';
                serviceLogin.getLogin(formData.username, formData.password).success(function (data) {
                    if (data == null) {
                        $scope.errorlogin = true;
                        $scope.msgerror = 'Clave incorrecta';
                        $scope.inlogin = true;
                        $scope.labellogin = "Ingresar";
                    }
                    else {
                        $window.location.href = "/Home/AutorizarIngreso?idUser=" + formData.username;
                    }
                }).error(function (data) {
                    $scope.errorlogin = true;
                    $scope.msgerror = data;
                    $scope.inlogin = true;
                    $scope.labellogin = "Ingresar";
                });
            }

            $scope.Test = function () {
                serviceLogin.test(1).success(function (data) {
                    console.log(data);
                });
            }

            $scope.EnviarMail = function () {
                $scope.showerror = false;
                $scope.showsuccess = false;
                $scope.showenviando = false;
                $scope.msgerr = "";
                $scope.inenvioclave = false;
                $scope.okenvio = false;
                $scope.labelloginsendclave = "Enviando";

                console.log("mail informado: " + $scope.usernameforgot);

                $scope.showenviando = true;
                serviceLogin.sendMail($scope.usernameforgot).success(function (data) {
                    console.log(JSON.stringify(data));
                    $scope.okenvio = true;
                    $scope.inenvioclave = true;
                    $scope.labelloginsendclave = "Enviar";

                    setTimeout(function () {
                        $("#myModal .close").click()
                    }, 3000);
                }).error(function (data) {
                    $scope.msgerr = data;
                    $scope.showerror = true;
                    $scope.showenviando = false;
                });
            }

            $scope.Instalar = function (formData) {
                $('#load').modal({ backdrop: 'static', keyboard: false })
                $scope.lblLoad = "Creando base de datos del sistema...";
                serviceLogin.CrearBD(formData.userbd, formData.passbd, formData.svbd, formData.nombrebd).success(function (dataCrearBD) {
                    $scope.lblLoad = "Creando Super Usuario...";
                    serviceLogin.CrearSuper(formData.nombreuser, formData.apellidouser, formData.mailuser).success(function (dataCrearSuper) {
                        $scope.lblLoad = "Guardando configuracion general...";
                        serviceLogin.GuardarConfig(formData.innosetup, formData.smtpwu, formData.mailwu, formData.passmailwu, formData.aliasmailwu, formData.mailsoporte, formData.dirupload, formData.dirvoficial, formData.dirfuentes).success(function (dataConfig) {
                            $scope.lblLoad = "Redireccionando...";
                            $timeout(function () {
                                $window.location.href = "/Home";
                            }, 3000);
                        }).error(function (err) {
                            console.error(err);
                            $scope.lblLoad = "Ocurrió un error al intentar guardar la configuración principal, verifique consola del navegador.";
                        });
                    }).error(function (err) {
                        console.error(err);
                        $scope.lblLoad = "Ocurrió un error al intentar crear al super usuario, verifique consola del navegador.";
                    });
                }).error(function (err) {
                    console.error(err);
                    $scope.lblLoad = "Ocurrió un error al intentar crear la base de datos, verifique consola del navegador.";
                });
            }

        }

    }
})();
