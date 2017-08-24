/// <reference path="C:\Users\Administrador\Documents\Visual Studio 2015\Projects\winperupdate\WinPerUpdateAdmin\Views/Admin/CrearVersion.cshtml" />
(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceAdmin', serviceAdmin);

    serviceAdmin.$inject = ['$http', '$q', '$window'];

    function serviceAdmin($http, $q, $window) {
        var service = {
            getVersiones: getVersiones,

            addVersion: addVersion,
            addVersionInicial: addVersionInicial,
            updVersion: updVersion,
            getVersion: getVersion,
            genVersion: genVersion,
            getClientes: getClientes,
            updEstadoVersion: updEstadoVersion,

            getModulos: getModulos,
            getUltimaRelease: getUltimaRelease,
            getComponentesOficiales: getComponentesOficiales,
            getTiposComponentes: getTiposComponentes,
            existeComponente: existeComponente,
            getComponentesByName: getComponentesByName,
            getComponentesVersion: getComponentesVersion,

            getComponente: getComponente,
            addComponente: addComponente,
            updComponente: updComponente,
            delComponente: delComponente,

            addCliente: addCliente,
            GenVersionInicial: GenVersionInicial,
            addClientesToVersion: addClientesToVersion,

            getModulosByComponente: getModulosByComponente,
            getModulosVersion: getModulosVersion,

            addControlCambios: addControlCambios,
            getControlCambios: getControlCambios,
            getControlCambiosEx: getControlCambiosEx,
            delDocCambios: delDocCambios,
            updControlCambios: updControlCambios,
            delControlCambios: delControlCambios,
            isModuloVigente: isModuloVigente,
            addComponenteSql: addComponenteSql,
            ccOK: ccOK,
            getExisteRelease: getExisteRelease
        };

        return service;


        function getExisteRelease(release) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ExisteRelease?release='+release,
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

        function ccOK(version) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/modCCFaltantes/'+version,
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

        function isModuloVigente(fileName) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/isModuloVigente/?fileName=' + fileName,
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

        function delControlCambios(version, tips, modulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/delControlCambios/'+version+'/'+tips+'/'+modulo,
                type: "DELETE",
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

        function delDocCambios(doc, version, tips, modulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/delDocCambios/'+version+'/'+tips+'/'+modulo+'/?doc='+doc,
                type: "DELETE",
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

        function getControlCambiosEx(idVersion, tips, modulo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/getControlCambios/' + idVersion + '/'+tips + '/'+modulo,
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

        function getControlCambios(idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/getControlCambios/'+idVersion,
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

        function addControlCambios(idVersion, idModulo, release, tips, desccc, docCambios, impacto) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var obj = {
                "Version": idVersion,
                "Modulo":idModulo,
                "Release":release,
                "Tips":tips,
                "Descripcion":desccc,
                "DocCambios": docCambios,
                "Impacto": impacto
            };

            $.ajax({
                url: '/api/ControlCambios',
                type: "POST",
                dataType: 'Json',
                data: obj,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('msgerror');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function updControlCambios(idVersion, idModulo, release, tips, desccc, docCambios, impacto) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var obj = {
                "Version": idVersion,
                "Modulo": idModulo,
                "Release": release,
                "Tips": tips,
                "Descripcion": desccc,
                "DocCambios": docCambios,
                "Impacto": impacto
            };

            $.ajax({
                url: '/api/ControlCambios',
                type: "PUT",
                dataType: 'Json',
                data: obj,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('msgerror');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                
        function getModulosVersion(idversion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/getModulosVersion/'+idversion,
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

        function getModulosByComponente(filename) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var DirModulos = {
                "Directorio":filename
            }

            $.ajax({
                url: '/api/getModulosByComponente',
                type: "Post",
                dataType: 'Json',
                data: DirModulos,
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

        function getComponentesVersion(idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ComponenteOkSegunVersion/' + idVersion,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen componentesOk');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No existen componentesOk');
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
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
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

        function getComponentesByName(NomComponente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Componentes/' + NomComponente+'/Comentario',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene Componentes creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene Componentes creadas');
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

        function getTiposComponentes(idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/TipoComponentes/'+idVersion,
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene TipoComponentes creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene TipoComponentes creadas');
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

        function existeComponente(nombreComponente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/Componentes/' + nombreComponente+'/Existe',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene componentes creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene componentes creadas');
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

        function getComponentesOficiales() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/ComponentesOficiales',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene componentes creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene componentes creadas');
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

        function getUltimaRelease() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/UltimaRelease',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene versiones creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene versiones creadas');
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

        function getVersiones() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene versiones creadas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No tiene versiones creadas');
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

        function GenVersionInicial(idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/VersionInicial/' + idVersion,
                type: "POST",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la version');
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

        function updEstadoVersion(idVersion, estado) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/'+idVersion+'/Estado/'+estado+'/Upd',
                type: "PUT",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo cambiar el estado de la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo cambiar el estado de la version');
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

        function addVersionInicial(release, estado, comentario, hasdeploy) {
            var deferred = $q.defer();
            var promise = deferred.promise;


            var version = {
                "Release": release,
                "Estado": estado,
                "Comentario": comentario,
                "Fecha": "null",
                "IsVersionInicial": true,
                "HasDeploy31": hasdeploy
            };

            $.ajax({
                url: '/api/Version',
                type: "POST",
                dataType: 'Json',
                data: version,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la version');
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

        function addVersion(release, fecha, estado, comentario, usuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var fechaIn = fecha.split('/');

            var version = {
                "Release": release,
                "Fecha": '' + fechaIn[2] + '-' + fechaIn[1] + '-' + fechaIn[0] + 'T00:00:00',
                "Estado": estado,
                "Comentario": comentario,
                "Usuario": usuario,
                "IsVersionInicial": false
            };
            console.debug(JSON.stringify(version));

            $.ajax({
                url: '/api/Version',
                type: "POST",
                dataType: 'Json',
                data: version,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la version');
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

        function updVersion(id, release, fecha, estado, comentario, usuario, instalador, hasdeploy) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var fechaIn = [];
            fechaIn = fecha.split('/');
            if (fechaIn.length != 3) fechaIn = fecha.split('-');

            var version = {
                "IdVersion": id,
                "Release": release,
                "Fecha": '' + fechaIn[2] + '-' + fechaIn[1] + '-' + fechaIn[0] + 'T00:00:00',
                "Estado": estado,
                "Comentario": comentario,
                "Usuario": usuario,
                "Instalador": instalador,
                "HasDeploy31": hasdeploy
            };
            //console.debug(JSON.stringify(version));

            $.ajax({
                url: '/api/Version/' + id,
                type: "PUT",
                dataType: 'Json',
                data: version,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo modificar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo modificar la version');
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

        function getVersion(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/' + id,
                type: "get",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No existe la version');
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

        function genVersion(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var version = {
                "idVersion": id
            };
            console.debug(JSON.stringify(version));

            $.ajax({
                url: '/Admin/GenerarVersion',
                type: "POST",
                dataType: 'Json',
                data: version,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo generar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo generar la version');
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

        function addComponenteSql(id, modulo, name, fecha, tipo, motor) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var componente = {
                "Modulo": modulo,
                "Name": name,
                "LastWrite": fecha,
                "Version": "S/I",
                "Tipo": tipo,
                "MotorSql": motor
            };

            $.ajax({
                url: '/api/Version/' + id + '/Componentes',
                type: "POST",
                dataType: 'Json',
                data: componente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la componente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la componente');
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

        function addComponente(id, modulo, name, fecha, version) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var componente = {
                "Modulo": modulo,
                "Name": name,
                "LastWrite": fecha,
                "Version": version,
                "Tipo": '*',
                "MotorSql": 'N/A'
            };

            $.ajax({
                url: '/api/Version/' + id + '/Componentes',
                type: "POST",
                dataType: 'Json',
                data: componente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la componente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la componente');
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

        function addClientesToVersion(id, listaClientes, tipoPub) {
            var deferred = $q.defer();
            var promise = deferred.promise;


            $.ajax({
                url: '/api/Version/' + id + '/Clientes/TipoPub/' + tipoPub,
                type: "POST",
                dataType: 'text',
                contentType: "application/json",
                data: JSON.stringify(listaClientes),
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version al cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la version al cliente');
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

        function addCliente(id, idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/' + id + '/Cliente/' + idCliente,
                type: "POST",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version al cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo agregar la version al cliente');
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

        function updComponente(id, name, modulo, comentario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var componente = {
                "Modulo": modulo,
                "Name": name,
                "Comentario": comentario
            };

            $.ajax({
                url: '/api/Version/' + id + '/Componentes',
                type: "PUT",
                dataType: 'Json',
                data: componente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo modificar la componente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo modificar la componente');
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

        function delComponente(id, name) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var componente = {
                "Name": name
            };

            $.ajax({
                url: '/api/Version/' + id + '/Componentes',
                type: "DELETE",
                dataType: 'Json',
                data: componente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo eliminar la componente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No se pudo eliminar la componente');
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

        function getModulos() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulos',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen módulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No existen módulos');
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

        function getComponente(id, name) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/' + id + '/Componentes/' + name + '/nameFile',
                type: "get",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe la componente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + ". msg: " + xhr.responseText);
                    deferred.reject('No existe la componente');
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