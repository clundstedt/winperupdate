﻿(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceSU', serviceSU);

    serviceSU.$inject = ['$http', '$q'];

    function serviceSU($http, $q) {
        var service = {
            LoadWebConf: LoadWebConf,
            Guardar: Guardar
        };

        return service;

        function LoadWebConf(){
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Config',
                type: "GET",
                dataType: 'Json',
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

        function Guardar(innosetup, smtpwu, mailwu, passmailwu, aliasmailwu, mailsoporte, dirupload, dirvoficial, dirfuentes) {
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

    }
})();