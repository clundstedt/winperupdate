(function () {
    'use strict';

    angular
        .module('app')
        .factory('factoryHome', factoryHome);

    factoryHome.$inject = ['$http', '$q'];

    function factoryHome($http, $q) {
        var service = {
            getUsuarioSession: getUsuarioSession,
            compruebaPwd: compruebaPwd
        };

        return service;

        function getUsuarioSession(id) {
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
                    console.error('error = ' + xhr.status);
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


        function compruebaPwd(mail, clave, nClave) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Seguridad?mail=' + mail + '&password=' + clave + '&nPassword=' + nClave,
                type: "PUT",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('Usuario No existe');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('Usuario No existe');
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