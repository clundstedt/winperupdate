
(function () {
    'use strict';

    angular.module('app', [
        'ngRoute'
        , 'smart-table'
        , 'smart-pagination'
    ])
    .config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/Bitacora/Clientes',
            controller: 'ctrlBitacora'
        }).
        when('/Modulo/:menu', {
            templateUrl: '/Bitacora/Modulo',
            controller: 'ctrlBitacora'
        }).
        when('/Usuario/:menu', {
            templateUrl: '/Bitacora/Usuario',
            controller: 'ctrlBitacora'
        }).
        when('/Version/:menu', {
            templateUrl: '/Bitacora/Version',
            controller: 'ctrlBitacora'
        }).
        otherwise({
            redirectTo: '/'
        });

    })
    ;
})();
