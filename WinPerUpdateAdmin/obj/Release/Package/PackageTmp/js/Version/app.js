(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
         'ngRoute'
        ,'angularFileUpload'
        , 'smart-table'
        // Custom modules 
        , 'smart-pagination'
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
        when('/PublicarParcial/:idVersion', {
            templateUrl: '/Admin/PubParcial',
            controller: 'publicar'
        }).
        when('/CrearComponente/:idVersion', {
            templateUrl: '/Admin/CrearComponente',
            controller: 'componente'
        }).
        when('/EditComponente/:idVersion/:name', {
            templateUrl: '/Admin/EditComponente',
            controller: 'editcomponente'
        }).
        when('/ControlCambios/:idVersion', {
            templateUrl: '/Admin/ControlCambios',
            controller: 'controlcambios'
        }).
        when('/ControlCambios/:idVersion/:tips/:modulo', {
            templateUrl: '/Admin/ControlCambios',
            controller: 'controlcambios'
        }).
        when('/AsignarScripts/:idVersion', {
            templateUrl: '/Admin/AsignarScripts',
            controller: 'asignarscripts'
        }).
        otherwise({
            redirectTo: '/'
        });

    })
    .filter('filtroVersionesLiberadas', function () {
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

    .filter('filtroVersionesNV', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (version) {
                if (version.Estado == 'C') {
                    salida.push(version)
                }
            });
            return salida;
        }
    })

    .filter('filtroVersionesPub', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (version) {
                if (version.Estado == 'P') {
                    salida.push(version)
                }
            });
            return salida;
        }
    })
    .filter('filtroComponente', function () {
        return function (input, ex) {
            var salida = [];
            angular.forEach(input, function (componente) {
                if (ex.includes(componente.componente.Extension)) {
                    salida.push(componente)
                }
            });
            return salida;
        }
    })
    .filter('filtroModulosOk', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (item) {
                if (item.Estado == 'V') {
                    salida.push(item)
                }
            });
            return salida;
        }
    })

    .filter('filtroClientesNoSel', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (item) {
                if (item.Tipo == 0) {
                    salida.push(item)
                }
            });
            return salida;
        }
    })

    .filter('filtroClientesSel', function () {
        return function (input) {
            var salida = [];
            angular.forEach(input, function (item) {
                if (item.Tipo == 1) {
                    salida.push(item)
                }
            });
            return salida;
        }
    })
    ;


})();