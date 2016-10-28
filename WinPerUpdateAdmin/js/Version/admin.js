(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'serviceAdmin'];

    function admin($scope, serviceAdmin) {
        $scope.title = 'admin';

        activate();

        function activate() {
            $scope.versiones = [];
           

            serviceAdmin.getVersiones().success(function (data) {
                $scope.versiones = data;
            }).
            error(function (data) {
                console.error(data);
            });
        }
    }
})();
