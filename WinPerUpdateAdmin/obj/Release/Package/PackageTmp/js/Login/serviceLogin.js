(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceLogin', serviceLogin);

    serviceLogin.$inject = ['$http', '$q', '$window'];

    function serviceLogin($http, $q, $window) {
        var service = {
            getLogin: getLogin,
            sendMail: sendMail,
            CrearBD: CrearBD,
            CrearSuper: CrearSuper,
            GuardarConfig: GuardarConfig
        };

        return service;

        function CrearBD(userbd, passbd, svbd, nombrebd, nombreuser, apellidouser, mailuser, passsu) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var bddata = {
                "userbd": userbd,
                "passbd": passbd,
                "svbd": svbd,
                "nombrebd": nombrebd,
                "Usuario": {
                    "CodPrf": 5,
                    "Clave": passsu,
                    "Persona": {
                        "Apellidos": apellidouser,
                        "Nombres": nombreuser,
                        "Mail": mailuser
                    },
                    "EstUsr": "V"
                }
            };

            $.ajax({
                url: '/api/CrearBD',
                type: "POST",
                dataType: 'Json',
                data: bddata,
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
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function CrearSuper(nombreuser, apellidouser, mailuser) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var usuario = {
                "CodPrf": 5,
                "Persona": {
                    "Apellidos": apellidouser,
                    "Nombres": nombreuser,
                    "Mail": mailuser
                },
                "EstUsr": "V"
            };

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
                        deferred.reject('Usuario No existe');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function GuardarConfig(innosetup, smtpwu, mailwu, passmailwu, aliasmailwu, mailsoporte, dirupload, dirvoficial, dirfuentes) {
            var webConfig = {
                "pathGenSetup": innosetup,
                "hostMail": smtpwu,
                "userMail": mailwu,
                "pwdMail": passmailwu,
                "fromMail": aliasmailwu,
                "correoSoporte": mailsoporte,
                "upload": dirupload,
                "voficial": dirvoficial,
                "fuentes": dirfuentes
            };

            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/GuardarConfig',
                type: "PUT",
                dataType: 'Json',
                data: webConfig,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe configuracion');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existe configuracion');
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
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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