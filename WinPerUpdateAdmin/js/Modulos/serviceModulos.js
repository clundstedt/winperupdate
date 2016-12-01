(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceModulos', serviceModulos);

    serviceModulos.$inject = ['$http', '$q'];

    function serviceModulos($http, $q) {
        var service = {
            listarModulos: listarModulos,
            getModulosXlsx: getModulosXlsx,
            getModulo: getModulo,
            getComponentesModulo: getComponentesModulo,
            getTipoComponentes: getTipoComponentes,

            setVigente: setVigente,

            addModulo: addModulo,
            updModulo: updModulo,
            delModulo: delModulo,

            addComponentesModulos: addComponentesModulos,
            addTipoComponentes: addTipoComponentes,
            delTipoComponentes: delTipoComponentes,
            delComponentesModulos: delComponentesModulos,
            updComponentesModulos: updComponentesModulos
        };

        return service;

        function setVigente(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: 'api/Modulo/'+idModulo+'/Vigente',
                type: "PUT",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe modulos');
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

        function getTipoComponentes() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/TipoComponentes',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe Tipo Componentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe Tipo Componentes');
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

        function getComponentesModulo(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/' + idModulo+'/Componentes',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe Componentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe Componentes');
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

        function addComponentesModulos(nombre, descripcion, idModulo, tipocomponente) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var ComponentesModulos = {
                "Nombre": nombre,
                "Descripcion": descripcion,
                "TipoComponentes": {
                    "idTipoComponentes": tipocomponente
                }
            };

            console.debug(JSON.stringify(ComponentesModulos));
            $.ajax({
                url: '/api/ComponentesModulos/' + idModulo,
                type: "POST",
                dataType: 'Json',
                data: ComponentesModulos,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe ComponentesModulos');
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

        function delComponentesModulos(idComponentesModulos) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ComponentesModulos/' + idComponentesModulos,
                type: "DELETE",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe ComponentesModulos');
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

        function updComponentesModulos(idComponentesModulos, Nombre, Descripcion, TipoComponentes) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var ComponentesModulos = {
                "Nombre": Nombre,
                "Descripcion": Descripcion,
                "TipoComponentes": {
                    "idTipoComponentes": TipoComponentes
                }
            };

            $.ajax({
                url: '/api/ComponentesModulos/' + idComponentesModulos,
                type: "PUT",
                dataType: 'Json',
                data: ComponentesModulos,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe ComponentesModulos');
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

        function addTipoComponentes(nombre, iscompbd){
            var deferred = $q.defer();
            var promise = deferred.promise;
            var TipoComponentes = {
                "Nombre": nombre,
                "isCompBD": iscompbd
            };
            $.ajax({
                url: '/api/TipoComponentes',
                type: "POST",
                dataType: 'Json',
                data: TipoComponentes,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe TipoComponentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe TipoComponentes');
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

        function delTipoComponentes(idTipoComponentes) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/TipoComponentes/' + idTipoComponentes,
                type: "DELETE",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe TipoComponentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe TipoComponentes');
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

        function getModulo(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/' + idModulo,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe modulos');
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

        function getModulosXlsx(idUsuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ModulosXlsx/'+idUsuario,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen modulos');
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

        function listarModulos() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulos',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen modulos');
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

        function addModulo(nombre, descripcion, iscore, directorio) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var modulo = {
                "NomModulo": nombre,
                "Descripcion": descripcion,
                "isCore": iscore,
                "Directorio": directorio
            }

            $.ajax({
                url: '/api/Modulo',
                type: "POST",
                dataType: 'Json',
                data: modulo,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen modulos');
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

        function updModulo(id, nombre, descripcion, iscore, directorio) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var modulo = {
                "NomModulo": nombre,
                "Descripcion": descripcion,
                "isCore": iscore,
                "Directorio": directorio
            }

            $.ajax({
                url: '/api/Modulo/'+id,
                type: "PUT",
                dataType: 'Json',
                data: modulo,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen modulos');
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

        function delModulo(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/'+idModulo,
                type: "DELETE",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existen modulos');
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