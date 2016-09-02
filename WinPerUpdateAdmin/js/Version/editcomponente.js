(function () {
    'use strict';

    angular
        .module('app')
        .controller('editcomponente', editcomponente);

    editcomponente.$inject = ['$scope', '$routeParams', 'serviceAdmin'];

    function editcomponente($scope, $routeParams, serviceAdmin) {
        $scope.title = 'editcomponente';

        activate();

        function activate() {
            console.log(JSON.stringify($routeParams));
            $scope.idVersion = $routeParams.idVersion;
            $scope.name = $routeParams.name;

            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
            }).error(function (data) {
                console.debug(data);
            });

        }
    }
})();
