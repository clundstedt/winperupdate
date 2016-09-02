(function () {
    'use strict';

    angular
        .module('app')
        .controller('version', version);

    version.$inject = ['$scope', '$window', '$routeParams', 'serviceAdmin'];

    function version($scope, $window, $routeParams, serviceAdmin) {
        $scope.title = 'version';

        activate();

        function activate() {
            $scope.idversion = 0;
            $scope.increate = true;
            $scope.titulo = "Crear Versión";
            $scope.labelcreate = "Crear";
            $scope.componentes = [];
            $scope.totales = [0, 0, 0];
            $scope.formData = {};
            $scope.fechaini = '';

            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idversion = $routeParams.idVersion;
                $scope.titulo = "Modificar Versión";

                serviceAdmin.getVersion($scope.idversion).success(function (data) {
                    $scope.formData.release = data.Release;
                    $scope.formData.fecha = data.FechaFmt;
                    $scope.componentes = data.Componentes;

                    $scope.fechaini = data.FechaFmt;

                    angular.forEach($scope.componentes, function (item) {
                        if (item.Tipo == 'exe') $scope.totales[0]++;
                        else if (item.Tipo == 'qrp') $scope.totales[1]++;
                        else $scope.totales[2]++;
                    });

                }).error(function (data) {
                    console.error(data);
                });
            }
        }

        $scope.SaveVersion = function (formData) {
            console.log(JSON.stringify(formData));

            serviceAdmin.addVersion(formData.release, formData.fecha, 'N').success(function (data) {
                $window.location.href = "/EditVersion/" + data.idVersion;
            }).error(function (data) {
                console.error(data);
            });
        };
    }
})();
