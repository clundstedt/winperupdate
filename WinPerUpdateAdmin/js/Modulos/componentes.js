(function () {
    'use strict';

    angular
        .module('app')
        .controller('componentes', componentes);

    componentes.$inject = ['$scope']; 

    function componentes($scope) {
        $scope.title = 'componentes';

        activate();

        function activate() { }
    }
})();
