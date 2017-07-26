(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 
        
        // 3rd Party Modules
        , 'angularFileUpload'
        , 'smart-table'
        // Custom modules 
        , 'smart-pagination'
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