(function () {
    'use strict';

    angular
        .module('app')
        .service('serviceLogin', serviceLogin);

    serviceLogin.$inject = ['$http'];

    function serviceLogin($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData() { }
    }
})();