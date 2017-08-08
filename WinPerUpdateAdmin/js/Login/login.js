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
            $scope.msgError = "";

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
                        $window.sessionStorage.token = window.btoa(data.GetToken);
                        $window.location.href = "/Home/AutorizarIngreso?idUser=" + formData.username;
                    }
                }).error(function (err) {
                    $scope.errorlogin = true;
                    $scope.msgerror = data;
                    $scope.inlogin = true;
                    $scope.labellogin = "Ingresar";
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
                    console.log(data);
                    $scope.okenvio = true;
                    $scope.inenvioclave = true;
                    $scope.labelloginsendclave = "Enviar";

                    setTimeout(function () {
                        $("#myModal .close").click()
                    }, 3000);
                }).error(function (err) {
                    $scope.msgerr = data;
                    $scope.showerror = true;
                    $scope.showenviando = false;
                });
            }

            $scope.Instalar = function (formData) {
                $('#load').modal({ backdrop: 'static', keyboard: false })
                $scope.lblLoad = "Creando base de datos del sistema y Super Usuario...";
                serviceLogin.CrearBD(formData.userbd, formData.passbd, formData.svbd, formData.nombrebd, formData.nombreuser, formData.apellidouser, formData.mailuser, formData.passsu).success(function (dataCrearBD) {
                    $scope.lblLoad = "Redireccionando...";
                    $timeout(function () {
                        $window.location.href = "/Home";
                    }, 3000);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                    $scope.lblLoad = "Ocurrió un error al intentar crear la base de datos, verifique consola del navegador.";
                });
            }

        }

    }
})();
