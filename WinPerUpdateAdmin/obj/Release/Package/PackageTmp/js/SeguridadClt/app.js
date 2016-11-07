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
            templateUrl: '/SeguridadClt/Usuarios',
            controller: 'seguridad'
        }).
        when('/EditCliente/:idCliente/Usuario', {
            templateUrl: '/SeguridadClt/Usuario',
            controller: 'seguridad'
        }).
        when('/EditCliente/:idCliente/Usuario/:idUsuario', {
            templateUrl: '/SeguridadClt/Usuario',
            controller: 'seguridad'
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