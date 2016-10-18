(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceClientes', serviceClientes);

    serviceClientes.$inject = ['$http', '$q'];

    function serviceClientes($http, $q) {
        var service = {
            getRegiones: getRegiones,
            getComunas: getComunas,
            getClientes: getClientes,
            getCliente: getCliente,
            getUsuarios: getUsuarios,
            getUsuario: getUsuario,

            addCliente: addCliente,
            addUsuario: addUsuario,
            updCliente: updCliente,
            updUsuario: updUsuario,
            delCliente: delCliente
        };

        return service;

        function getRegiones() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Region',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen regiones');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen regiones');
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

        function getComunas(idRgn) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Region/' + idRgn + '/Comunas',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen comunas asociadas a la región');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen comunas asociadas a la región');
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

        function getClientes() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen clientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen clientes');
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
                url: '/api/Clientes/' + id,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe cliente');
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

        function getUsuarios(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id + '/Usuarios',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen usuarios');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
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

        function getUsuario(id, idUsuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id + '/Usuarios/' + idUsuario,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe el usuario');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe el usuario');
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

        function addCliente(rut, dv, nombre, direccion, idCmn) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var cliente = {
                "Rut": rut,
                "Dv": dv,
                "Nombre": nombre,
                "Direccion": direccion,
                "Comuna": {
                    "idCmn": idCmn
                }
            };
            console.debug(JSON.stringify(cliente));

            $.ajax({
                url: '/api/Clientes',
                type: "POST",
                dataType: 'Json',
                data: cliente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
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

        function addUsuario(idCliente, codprf, apellidos, nombres, mail, estado) {
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
            //console.debug(JSON.stringify(usuario));

            $.ajax({
                url: '/api/Clientes/' + idCliente + '/Usuarios',
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
                    console.error('error = ' + xhr.status);
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

        function updCliente(id, rut, dv, nombre, direccion, idCmn) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var cliente = {
                "Rut": rut,
                "Dv": dv,
                "Nombre": nombre,
                "Direccion": direccion,
                "Comuna": {
                    "idCmn": idCmn
                }
            };
            console.debug(JSON.stringify(cliente));

            $.ajax({
                url: '/api/Clientes/' + id,
                type: "PUT",
                dataType: 'Json',
                data: cliente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo modificar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudo modificar el cliente');
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

        function updUsuario(id, idUsuario, codprf, idPer, apellidos, nombres, mail, estado) {
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
            console.debug(JSON.stringify(usuario));

            $.ajax({
                url: '/api/Clientes/' + id + '/Usuarios/' + idUsuario,
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
                    console.error('error = ' + xhr.status);
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

        function delCliente(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id,
                type: "DELETE",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo eliminar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
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