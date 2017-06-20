(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        , 'smart-table'
        , 'ui.select'
        , 'smart-pagination'
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
        when('/EditCliente/:idCliente/Usuario', {
            templateUrl: '/Clientes/Usuario',
            controller: 'usuarios'
        }).
        when('/EditCliente/:idCliente', {
            templateUrl: '/Clientes/Crear',
            controller: 'clientes'
        }).
        when('/EditCliente/:idCliente/Usuario/:idUsuario', {
            templateUrl: '/Clientes/Usuario',
            controller: 'usuarios'
        }).
        otherwise({
            redirectTo: '/'
        });

    })

    .filter('filtroPerfil', function () {
        return function (input, idPrf) {
            var salida = [];
            angular.forEach(input, function (item) {
                if (item.CodPrf == idPrf) {
                    salida.push(item)
                }
            });
            return salida;
        }
    })
;
})();