(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceLogin', serviceLogin);

    serviceLogin.$inject = ['$http', '$q'];

    function serviceLogin($http, $q) {
        var service = {
            getLogin: getLogin,
            sendMail: sendMail,
            test: test
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

        function test(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var archivo = [
                {
                    "Name": "Componente 1",
                    "DateCreate": "2016-09-30T10:20:30",
                    "Length": 1000,
                    "Version": "1.0",
                    "Modulo": "winper",
                    "Comentario": "esta es una prueba"
                },
                {
                "Name": "Componente 2",
                "DateCreate": "2016-09-30T10:20:30",
                "Length": 2000,
                "Version": "2.0",
                "Modulo": "winper",
                "Comentario": "esta es una prueba 2"
                }	
                ];

            $.ajax({
                url: 'api/Version/' + id + '/LComponentes',
                type: "POST",
                dataType: "json",
                data: archivo,
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