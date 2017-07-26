
(function () {
    'use strict';

    angular
        .module('app')
        .controller('ctrlBitacora', ctrlBitacora);

    ctrlBitacora.$inject = ['$scope', '$window','$routeParams', 'svcBitacora', '$timeout'];

    function ctrlBitacora($scope, $window, $routeParams, svcBitacora, $timeout) {
        $scope.title = 'titulo';

        activate();

        function activate() {
            $scope.registrosBitacora = [];
            $scope.msgError = "";
            $scope.msgSuccess = "";
            $scope.usuario = null;

            if (!jQuery.isEmptyObject($routeParams)) {
                svcBitacora.getBitacoraByMenu($routeParams.menu).success(function (data) {
                    $scope.registrosBitacora = data;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            } else {
                svcBitacora.getBitacoraByMenu("Cliente").success(function (data) {
                    $scope.registrosBitacora = data;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            }

            $scope.showMdlInfo = function (usuario) {
                svcBitacora.getUsuarioBitacora(usuario).success(function (data) {
                    $("#mdlInfoUser").modal('show');
                    $scope.usuario = data;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0); window.scrollTo(0, 0);
                });
            }

        }
    }
})();
