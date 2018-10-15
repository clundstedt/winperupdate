
(function () {
    'use strict';

    angular
        .module('app')
        .factory('svcBitacora', svcBitacora);

    svcBitacora.$inject = ['$http', '$q', '$window'];

    function svcBitacora($http, $q, $window) {
        var service = {
            getBitacoraByMenu: getBitacoraByMenu,
            getUsuarioBitacora: getUsuarioBitacora
        };

        return service;

        //Funciones API

        function getBitacoraByMenu(menu) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Bitacora/Menu?menu='+menu,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('msgerror');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + " msg = " + xhr.responseText);
                    deferred.reject('msgerror');
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

        function getUsuarioBitacora(usuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Usuarios/'+usuario,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('msgerror');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + " msg = " + xhr.responseText);
                    deferred.reject('msgerror');
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
