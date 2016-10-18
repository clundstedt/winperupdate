(function () {
    'use strict';

    angular
        .module('app')
        .controller('login', login);

    login.$inject = ['$scope','$window', 'serviceLogin']; 

    function login($scope, $window, serviceLogin) {
        $scope.title = 'login';
        $scope.formData = {};

        activate();

        function activate() {
            $scope.inlogin = true;
            $scope.inenvioclave = true;
            $scope.labellogin = "Ingresar";
            $scope.labelloginsendclave = "Enviar Clave";
            $scope.msgerror = "";

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

        }

    }
})();
