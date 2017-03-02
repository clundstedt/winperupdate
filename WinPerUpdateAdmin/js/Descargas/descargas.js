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

            $scope.descargas = [];

            serviceDescargas.LoadDescargas().success(function (data) {
                $scope.descargas = data;
            }).error(function (err) {
                console.error(err);
            });
            
            $scope.Descargar = function (file) {
                $window.location.href = '/api/Descargar/' + file.NombreArchivo + '/Directorio/' + file.Nombre + '/Down';
            }
            
        }
    }
})();
