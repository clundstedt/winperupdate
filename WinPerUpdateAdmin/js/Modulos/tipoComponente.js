(function () {
    'use strict';

    angular
        .module('app')
        .controller('tipoComponente', tipoComponente);

    tipoComponente.$inject = ['$scope']; 

    function tipoComponente($scope) {
        $scope.title = 'tipoComponente';

        activate();

        function activate() { }
    }
})();
