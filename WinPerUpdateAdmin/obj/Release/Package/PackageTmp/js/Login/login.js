(function () {
    'use strict';

    angular
        .module('app')
        .controller('login', login);

    login.$inject = ['$scope','$window']; 

    function login($scope, $window) {
        $scope.title = 'login';
        $scope.formData = {};

        activate();

        function activate() {
            $scope.inlogin = true;
            $scope.inenvioclave = true;
            $scope.labellogin = "Ingresar";
            $scope.labelloginsendclave = "Enviar Clave";
            $scope.msgerror = "";
        }

        $scope.CheckAccess = function (formData) {
            $scope.inlogin = false;
            $scope.labellogin = "Validando Ingreso";
            $scope.errorlogin = false;
            $scope.msgerror = '';

            $window.location.href = "/Home/AutorizarIngreso?idUser=" + formData.username;
        }
    }
})();
