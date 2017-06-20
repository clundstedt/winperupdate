(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        , 'smart-table'
        , 'angularFileUpload'
        , 'smart-pagination'
        
    ]).config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/Modulos/Inicio',
            controller: 'inicio'
        }).
        when('/CrearModulo', {
            templateUrl: '/Modulos/CrearModulo',
            controller: 'modulos'
        }).
        when('/EditarModulo/:idModulo', {
            templateUrl: '/Modulos/CrearModulo',
            controller: 'modulos'
        }).
        when('/EditarModulo/:idModulo/Componente/:idComponente', {
            templateUrl: '/Modulos/EditarComponente',
            controller: 'componentes'
        }).
        otherwise({
            redirectTo: '/'
        });

    });
})();