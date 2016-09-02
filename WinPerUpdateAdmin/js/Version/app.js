(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute', 'angularFileUpload'

        // Custom modules 

        // 3rd Party Modules
        
    ])

    .config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/Admin/Versiones',
            controller: 'admin'
        }).
        when('/CrearVersion', {
            templateUrl: '/Admin/CrearVersion',
            controller: 'version'
        }).
        when('/EditVersion/:idVersion', {
            templateUrl: '/Admin/CrearVersion',
            controller: 'version'
        }).
        when('/CrearComponente/:idVersion', {
            templateUrl: '/Admin/CrearComponente',
            controller: 'componente'
        }).
        when('/EditComponente/:idVersion/:name', {
            templateUrl: '/Admin/EditComponente',
            controller: 'editcomponente'
        }).
        otherwise({
            redirectTo: '/'
        });

    })

    .filter('filtroEjecutables', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (componente) {
                if (componente.Tipo == 'exe') {
                    salida.push(componente)
                }
            })
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
            })
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
            })
            return salida;
        }
    });
})();