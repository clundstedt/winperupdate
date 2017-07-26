(function () {
    'use strict';

    angular
        .module('app')
        .controller('descargas', descargas);

    descargas.$inject = ['$scope', '$routeParams', '$window', '$timeout', 'serviceDescargas'];

    function descargas($scope, $routeParams, $window, $timeout, serviceDescargas) {
        $scope.title = 'Descargas';

        activate();

        function activate() {
            $scope.msgError = "";

            $scope.descargas = [];

            serviceDescargas.LoadDescargas().success(function (data) {
                $scope.descargas = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });
            
            $scope.Descargar = function (file) {
                $window.location.href = '/api/Descargar/' + file.NombreArchivo + '/Directorio/' + file.Nombre + '/Down';
            }
            
        }
    }
})();
