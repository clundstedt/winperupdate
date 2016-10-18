(function () {
    'use strict';

    angular
        .module('app')
        .controller('seguridad', seguridad);

    seguridad.$inject = ['$scope']; 

    function seguridad($scope) {
        $scope.title = 'seguridad';

        activate();

        function activate() { }
    }
})();
