(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceLogin', serviceLogin);

    serviceLogin.$inject = ['$http', '$q'];

    function serviceLogin($http, $q) {
        var service = {
            getLogin: getLogin,
            sendMail: sendMail
        };

        return service;

        function getLogin(mail, clave) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Seguridad?mail=' + mail + '&password='+clave,
                type: "GET",
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

        function sendMail(mail) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/Home/SendMail?mail=' + mail,
                type: "POST",
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    if (data.CodErr == 0) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject(data.MsgErr);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudo enviar mail');
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