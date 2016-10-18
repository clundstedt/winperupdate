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
            templateUrl: '/AdminClt/Versiones',
            controller: 'admin'
        }).
        otherwise({
            redirectTo: '/'
        });

    })

    .filter('filtroVersionesLiberadas', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (version) {
                if (version.Estado == 'P' || version.Estado == 'C') {
                    salida.push(version)
                }
            });
            return salida;
        }
    })


    .filter('filtroVersionesBeta', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (version) {
                if (version.Estado == 'N') {
                    salida.push(version)
                }
            });
            return salida;
        }
    })
    ;
})();