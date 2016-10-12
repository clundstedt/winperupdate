(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        , 'smart-table'
    ])

    .config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/Clientes/Inicio',
            controller: 'inicio'
        }).
        when('/CrearCliente', {
            templateUrl: '/Clientes/Crear',
            controller: 'clientes'
        }).
        when('/EditCliente/:idCliente', {
            templateUrl: '/Clientes/Crear',
            controller: 'clientes'
        }).
        otherwise({
            redirectTo: '/'
        });

    });
})();