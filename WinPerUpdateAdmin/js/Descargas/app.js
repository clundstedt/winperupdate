(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
         'ngRoute'
        ,'angularFileUpload'
        , 'smart-table'
        // Custom modules 

        // 3rd Party Modules
        
    ])

    .config(function ($routeProvider) {
        $routeProvider.
        when('/', {
            templateUrl: '/Descargas/Descargas',
            controller: 'descargas'
        }).
        otherwise({
            redirectTo: '/'
        });

    })
    
    ;
})();