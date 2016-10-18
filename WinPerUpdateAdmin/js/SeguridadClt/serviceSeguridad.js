(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceSeguridad', serviceSeguridad);

    serviceSeguridad.$inject = ['$http'];

    function serviceSeguridad($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();