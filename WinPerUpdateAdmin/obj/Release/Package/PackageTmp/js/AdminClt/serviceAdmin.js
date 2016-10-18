(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceAdmin', serviceAdmin);

    serviceAdmin.$inject = ['$http', '$q'];

    function serviceAdmin($http, $q) {
        var service = {
            getCliente: getCliente,
            getVersiones: getVersiones
        };

        return service;

        function getVersiones(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id + '/Versiones',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene versiones creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene versiones creadas');
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

        function getCliente(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Usuarios/' + id + '/Cliente',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene cliente asignado');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene cliente asignado');
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