(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceSeguridad', serviceSeguridad);

    serviceSeguridad.$inject = ['$http', '$q', '$window'];

    function serviceSeguridad($http, $q, $window) {
        var service = {
            getUsuario: getUsuario
        };

        return service;

        function getUsuario(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Usuarios/' + id,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe usuario');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existe usuario');
                }

            });

            promise.success = function (fn) {
                promise.then(fn);
                return promise;
            }

            promise.error = function (fn) {
                promise.then(null, fn);
                return promise;
            }

            return promise;
        }

    }
})();