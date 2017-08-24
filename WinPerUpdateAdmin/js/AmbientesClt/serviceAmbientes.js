(function () {
    'use strict';

    angular
        .module('app')
        .service('serviceAmbientes', serviceAmbientes);

    serviceAmbientes.$inject = ['$http','$q', '$window'];

    function serviceAmbientes($http, $q, $window) {
        var service = {
            getAmbientes: getAmbientes,
            getAmbientesXlsx: getAmbientesXlsx,
            getAmbiente: getAmbiente,

            addAmbiente: addAmbiente,
            updAmbiente: updAmbiente,
            delAmbiente: delAmbiente
        };

        return service;

        function getAmbientesXlsx(idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/AmbientesXLSX',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen ambientesxlsx');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen ambientes');
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

        function getAmbiente(idCliente, idAmbiente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/Ambiente/'+idAmbiente,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen ambientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen ambientes');
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

        function getAmbientes(idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/'+idCliente+'/Ambiente',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen ambientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen ambientes');
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

        function addAmbiente(idCliente, Nombre, Tipo, ServerBd, Instancia, NomBd, UserDbo, PwdDbo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var ambiente = {
                "Nombre": Nombre,
                "Tipo": Tipo,
                "ServerBd": ServerBd,
                "Instancia": Instancia,
                "NomBd": NomBd,
                "UserDbo": UserDbo,
                "PwdDbo": PwdDbo
            };

            $.ajax({
                url: "api/Cliente/"+idCliente+"/Ambiente",
                type: "POST",
                dataType: 'Json',
                data: ambiente,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo agregar el cliente');
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

        function updAmbiente(idCliente, idAmbiente, Nombre, Tipo, ServerBd, Instancia, NomBd, UserDbo, PwdDbo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var ambiente = {
                "Nombre": Nombre,
                "Tipo": Tipo,
                "ServerBd": ServerBd,
                "Instancia": Instancia,
                "NomBd": NomBd,
                "UserDbo": UserDbo,
                "PwdDbo": PwdDbo
            };
            console.log(JSON.stringify(ambiente));
            $.ajax({
                url: "api/Cliente/" + idCliente + "/Ambiente/" + idAmbiente,
                type: "PUT",
                dataType: 'Json',
                data: ambiente,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo agregar el cliente');
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

        function delAmbiente(idCliente, idAmbiente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: "api/Cliente/" + idCliente + "/Ambiente/" + idAmbiente,
                type: "DELETE",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo eliminar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo eliminar el cliente');
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