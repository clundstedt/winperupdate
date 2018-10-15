(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceClientes', serviceClientes);

    serviceClientes.$inject = ['$http', '$q', '$window'];

    function serviceClientes($http, $q, $window) {
        var service = {
            getRegiones: getRegiones,
            getComunas: getComunas,
            getClientes: getClientes,
            getCliente: getCliente,
            getUsuarios: getUsuarios,
            getUsuario: getUsuario,
            getClientesVersion: getClientesVersion,
            getKey: getKey,
            getNCliente: getNCliente,
            getModulosBySuite: getModulosBySuite,
            getModulosCliente: getModulosCliente,
            getVersionesClientes: getVersionesClientes,
            getTrabPlantas: getTrabPlantas,
            getTrabHonorarios: getTrabHonorarios,
            getSuites: getSuites,
            getAnios: getAnios,
            getExisteMail:getExisteMail,
           
            GenCorrelativo: GenCorrelativo,
            GenVersionInicial: GenVersionInicial,
            addVersionInicial: addVersionInicial,
            addClienteToVersion: addClienteToVersion,
            ExisteVersionInicial: ExisteVersionInicial,
            EnviarBienvenida: EnviarBienvenida,

            addCliente: addCliente,
            addUsuario: addUsuario,
            updCliente: updCliente,
            updUsuario: updUsuario,
            delCliente: delCliente,

            addModuloCliente: addModuloCliente,
            getClienteNoVigente: getClienteNoVigente,
            getModulosDesdeSuite: getModulosDesdeSuite,
            getClientesPDF: getClientesPDF
        };

        return service;


        function getModulosDesdeSuite(suite) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/getModulosDesdeSuite/'+suite,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
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

        function getClienteNoVigente(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/getClienteNoVigente/'+id,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
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

        function getExisteMail(mail) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ExisteMail?mail='+mail,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
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
        
        function EnviarBienvenida(idUsuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Bienvenida/Usuario/' + idUsuario,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo EnviarBienvenida');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo EnviarBienvenida');
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

        function ExisteVersionInicial(idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/ExisteVersionInicial/Cliente/' + idCliente,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo Verificar Version Inicial');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No se pudo Verificar Version Inicial');
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
                url: '/api/VersionInicial/' + idVersion + '/Cliente',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function addVersionInicial(release, estado, comentario, hasDeploy) {
            var deferred = $q.defer();
            var promise = deferred.promise;


            var version = {
                "Release": release,
                "Estado": estado,
                "Comentario": comentario,
                "Fecha": "null",
                "IsVersionInicial": true,
                "HasDeploy31": hasDeploy
            };

            $.ajax({
                url: '/api/Version',
                type: "POST",
                dataType: 'Json',
                data: version,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function addClienteToVersion(id, idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/' + id + '/Cliente/' + idCliente,
                type: "POST",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la version al cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function GenCorrelativo(folio, mescon) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/GenCorrelativo/'+mescon+'/'+folio,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen Correlativo');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen Correlativo');
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

        function getAnios() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/GetAnios',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen Años');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen Años');
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
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

        function getTrabPlantas() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/TrabPlantas',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen TrabPlantas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen TrabPlantas');
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

        function getTrabHonorarios() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/TrabHonorarios',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen Honorarios');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen Honorarios');
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

        function getVersionesClientes(idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/VersionesInstaladas',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen versiones');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('No existen versiones');
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

        function getModulosCliente(idCliente) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/ModulosWinper',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen módulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function getModulosBySuite(idsuite) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Modulos/Suite/'+idsuite,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen módulos');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function getRegiones() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Region',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen regiones');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen comunas asociadas a la región');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen clientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function getClientesVersion(idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/' + idVersion + '/Clientes',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen usuarios');
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

        function getUsuario(id, idUsuario) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id + '/Usuarios/' + idUsuario,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe el usuario');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function addModuloCliente(idCliente, modulos) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/'+idCliente+'/Modulos',
                type: "POST",
                dataType: 'text',
                contentType: "application/json",
                data: JSON.stringify(modulos),
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('ERR:No se pudo agregar el cliente');
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

        function addCliente(rut, dv, nombre, direccion, idCmn, NroLicencia, NumFolio, estmtc, mesini, nrotrbc, nrotrbh, nrousr, mescon, correlativo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var cliente = {
                "Rut": rut,
                "Dv": dv,
                "Nombre": nombre,
                "Direccion": direccion,
                "Comuna": {
                    "idCmn": idCmn
                },
                "NroLicencia": NroLicencia,
                "NumFolio": NumFolio,
                "EstMtc": estmtc,
                "Mesini": mesini,
                "NroTrbc":nrotrbc,
                "NroTrbh":nrotrbh,
                "NroUsr": nrousr,
                "MesCon": mescon,
                "Correlativo": correlativo
            };
            console.debug(JSON.stringify(cliente));

            $.ajax({
                url: '/api/Clientes',
                type: "POST",
                dataType: 'Json',
                data: cliente,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('ERR:No se pudo agregar el cliente');
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.debug(JSON.stringify(data));
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

        function updCliente(id, rut, dv, nombre, direccion, idCmn, NroLicencia, NumFolio, estmtc, mesini, nrotrbc, nrotrbh, nrousr, mescon, correlativo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var cliente = {
                "Rut": rut,
                "Dv": dv,
                "Nombre": nombre,
                "Direccion": direccion,
                "Comuna": {
                    "idCmn": idCmn
                },
                "NroLicencia": NroLicencia,
                "NumFolio": NumFolio,
                "EstMtc": estmtc,
                "Mesini": mesini,
                "NroTrbc": nrotrbc,
                "NroTrbh": nrotrbh,
                "NroUsr": nrousr,
                "MesCon": mescon,
                "Correlativo": correlativo
            };
            console.debug(JSON.stringify(cliente));

            $.ajax({
                url: '/api/Clientes/' + id,
                type: "PUT",
                dataType: 'Json',
                data: cliente,
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo modificar el cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
                    deferred.reject('ERR: No se pudo modificar el cliente');
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
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
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

        function delCliente(id, est, motivo) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/Vigente?id=' + id + '&est=' + est+'&motivo='+motivo,
                type: "DELETE",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
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

        function getKey(folio, mescon, correlativo,estmtc,mesini,nrotrbc,nrotrbh,nrousr) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Key/'+folio+'/'+mescon+'/'+correlativo+'/'+estmtc+'/'+mesini+'/'+nrotrbc+'/'+nrotrbh+'/'+nrousr,
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo generar el Key para este cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function getNCliente() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/Key',
                type: "GET",
                dataType: 'Json',
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token);},success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen comunas asociadas a la región');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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

        function getClientesPDF() {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/PDF',
                type: "GET",
                beforeSend: function (xhr) { xhr.setRequestHeader("Authorization", "Basic " + $window.sessionStorage.token); }, success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        console.log('OK services getClientesPDF');
                        var file = new Blob([data], { type: 'application/pdf' });
                        var fileURL = URL.createObjectURL(file);
                        deferred.resolve(fileURL);
                    }
                    else {
                        deferred.reject('No existen clientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status + "msg = " + xhr.responseText);
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
    }
})();