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
        when('/EditVersion/:idVersion', {
            templateUrl: '/AdminClt/Componentes',
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

    .filter('filtroEjecutables', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (componente) {
                if (componente.Tipo == 'exe') {
                    salida.push(componente)
                }
            });
            return salida;
        }
    })

    .filter('filtroReportes', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (componente) {
                if (componente.Tipo == 'qrp') {
                    salida.push(componente)
                }
            });
            return salida;
        }
    })

    .filter('filtroOtros', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (componente) {
                if (componente.Tipo == 'otro') {
                    salida.push(componente)
                }
            });
            return salida;
        }
    })

    ;
})();