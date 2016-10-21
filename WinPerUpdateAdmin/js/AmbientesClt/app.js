(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        
    ])

    .config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/AmbientesClt/Ambientes',
            controller: 'ambientes'
        }).
        when('/EditAmbiente/:idCliente/:idAmbiente', {
            templateUrl: '/AmbientesClt/Crear',
            controller: 'mantenedor'
        }).
        when('/Crear/:idCliente', {
            templateUrl: '/AmbientesClt/Crear',
            controller: 'mantenedor'
        }).
        otherwise({
            redirectTo: '/'
        });

    })

    ;
})();