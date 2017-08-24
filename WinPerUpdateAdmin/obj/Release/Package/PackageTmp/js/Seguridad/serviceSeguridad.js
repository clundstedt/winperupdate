(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceSeguridad', serviceSeguridad);

    serviceSeguridad.$inject = ['$http', '$q', '$window'];

    function serviceSeguridad($http, $q, $window) {
        var service = {
            getUsuario: getUsuario,
            addUsuario: addUsuario,
            updUsuario: updUsuario,

            getUsuarios: getUsuarios,
            getPerfiles: getPerfiles
        };

        return service;


        function getPerfiles() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Perfiles/Internos',
                type: "GET",
                dataType: 'Json',
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

        function getUsuarios() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Usuarios',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existes usuarios');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen usuarios');
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

        function addUsuario(codprf, apellidos, nombres, mail, estado) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var usuario = {
                "CodPrf": codprf,
                "Persona": {
                    "Apellidos": apellidos,
                    "Nombres": nombres,
                    "Mail": mail
                },
                "EstUsr": estado
            };
            console.debug(JSON.stringify(usuario));

            $.ajax({
                url: '/api/Usuarios',
                type: "POST",
                dataType: 'Json',
                data: usuario,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el usuario');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo agregar el usuario');
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

        function updUsuario(id, codprf, idPer, apellidos, nombres, mail, estado) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var usuario = {
                "CodPrf": codprf,
                "Persona": {
                    "Id": idPer,
                    "Apellidos": apellidos,
                    "Nombres": nombres,
                    "Mail": mail
                },
                "EstUsr": estado
            };
            //console.debug(JSON.stringify(usuario));

            $.ajax({
                url: '/api/Usuarios/' + id,
                type: "PUT",
                dataType: 'Json',
                data: usuario,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo actualizar el usuario');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo actualizar el usuario');
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