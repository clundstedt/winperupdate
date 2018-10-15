(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceModulos', serviceModulos);

    serviceModulos.$inject = ['$http', '$q', '$window'];

    function serviceModulos($http, $q, $window) {
        var service = {
            listarModulos: listarModulos,
            getModulosXlsx: getModulosXlsx,
            getModulo: getModulo,
            getComponentesModulo: getComponentesModulo,
            getTipoComponentes: getTipoComponentes,
            getDirs: getDirs,
            getExisteComponenteEnDir: getExisteComponenteEnDir,
            getComponenteModulo:getComponenteModulo,

            setVigente: setVigente,
            syncComponentes: syncComponentes,

            addModulo: addModulo,
            updModulo: updModulo,
            delModulo: delModulo,

            addComponentesModulos: addComponentesModulos,
            addTipoComponentes: addTipoComponentes,
            delTipoComponentes: delTipoComponentes,
            delComponentesModulos: delComponentesModulos,
            updComponentesModulos: updComponentesModulos,

            updTipoComponente: updTipoComponente,
            getSuites: getSuites,
            ExistDirModulo: ExistDirModulo,
            GetComponentesDirectorio: GetComponentesDirectorio,
            addComponentesDir: addComponentesDir
        };

        return service;


        function getExisteComponenteEnDir(comp, dir) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ExisteComponenteEnDir?nombreComp=' + comp + ' &dir= ' + dir,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    } else {
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


        function getDirs(dir) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var DirModulos = {
                "Directorio": dir
            };
            $.ajax({
                url: '/api/getDir',
                type: "POST",
                dataType: 'Json',
                data: DirModulos,
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

        function addComponentesDir(idModulo, lista) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/ComponentesDir/'+idModulo,
                type: "POST",
                dataType: 'text',
                contentType: "application/json",
                data: JSON.stringify(lista),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el ComponentesDir');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('ERR:No se pudo agregar el ComponentesDir');
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

        function GetComponentesDirectorio(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/'+idModulo+'/ComponentesDir',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen ComponentesDirectorio');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen ComponentesDirectorio');
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

        function ExistDirModulo(directorio) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var DirModulos = {
                "Directorio": directorio
            };

            $.ajax({
                url: '/api/ExistDir/Modulo',
                type: "POST",
                dataType: 'Json',
                data: DirModulos,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen Directorio');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen Directorio');
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

        function getSuites() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Suites',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen Suites');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen Suites');
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

        function syncComponentes() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Componentes/Sync',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                        deferred.reject(xhr.responseText);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject(xhr.responseText);
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

        function updTipoComponente(idtipocomponente, nombre, extensiones, bd, dll, cambios) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var tipoComp = {
                idTipoComponentes: idtipocomponente,
                Nombre: nombre,
                Extensiones: extensiones,
                isCompBD: bd,
                isCompDLL: dll,
                isCompCambios: cambios
            };
            $.ajax({
                url: '/api/TipoComponentes/' + idtipocomponente,
                type: "PUT",
                dataType: 'Json',
                data: tipoComp,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe TipoComponente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existe TipoComponente');
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

        function setVigente(idModulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/'+idModulo+'/Vigente',
                type: "PUT",
                dataType: 'Json',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe Tipo Componentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe Componentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe ComponentesModulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function addTipoComponentes(nombre, iscompbd, iscompdll, extensiones, iscompcambios){
            var deferred = $q.defer();
            var promise = deferred.promise;
            var TipoComponentes = {
                "Nombre": nombre,
                "isCompBD": iscompbd,
                "isCompDLL": iscompdll,
                "Extensiones": extensiones,
                "isCompCambios": iscompcambios
            };
            $.ajax({
                url: '/api/TipoComponentes',
                type: "POST",
                dataType: 'Json',
                data: TipoComponentes,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe TipoComponentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe TipoComponentes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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


        function getComponenteModulo(idmodulo, comp, tipocomponente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulo/' + idmodulo + '/' + tipocomponente + '/Componente?Comp=' + comp,
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


        function listarModulos() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulos',
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
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function addModulo(nombre, descripcion, iscore, directorio, suite) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var modulo = {
                "NomModulo": nombre,
                "Descripcion": descripcion,
                "isCore": iscore,
                "Directorio": directorio,
                "Suite": suite
            }

            $.ajax({
                url: '/api/Modulo',
                type: "POST",
                dataType: 'Json',
                data: modulo,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function updModulo(id, nombre, descripcion, iscore, directorio, suite) {
            var deferred = $q.defer();
            var promise = deferred.promise;
            var modulo = {
                "NomModulo": nombre,
                "Descripcion": descripcion,
                "isCore": iscore,
                "Directorio": directorio,
                "Suite": suite
            }

            $.ajax({
                url: '/api/Modulo/'+id,
                type: "PUT",
                dataType: 'Json',
                data: modulo,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);
                },
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen modulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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