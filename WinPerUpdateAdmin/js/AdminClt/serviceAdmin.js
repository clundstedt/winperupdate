(function () {
    'use strict';

    angular
        .module('app')
        .factory('serviceAdmin', serviceAdmin);

    serviceAdmin.$inject = ['$http', '$q'];

    function serviceAdmin($http, $q) {
        var service = {
            getCliente: getCliente,
            getVersion: getVersion,
            getVersiones: getVersiones,
            getAmbientes: getAmbientes,
            getScript: getScript,

            existeTarea: existeTarea,
            ambienteOK: ambienteOK,
            existeTareaAtrasada: existeTareaAtrasada,
            detalleTareasNoEx: detalleTareasNoEx,
            reportarTareaAtrasada: reportarTareaAtrasada,
            reportarTodasTareas: reportarTodasTareas,

            addVersion: addVersion,
            addTarea: addTarea
        };

        return service;

        function reportarTodasTareas(tareas) {

            var deferred = $q.defer();
            var promise = deferred.promise;
            var tareasSend = [];

            for (var i = 0; i < tareas.length; i++) {
                var tareaSend = {
                    "idTareas": tareas[i].idTareas,
                    "idClientes": tareas[i].idClientes,
                    "Ambientes": {
                        "Nombre": tareas[i].Ambientes.Nombre
                    },
                    "Estado": tareas[i].Estado,
                    "idVersion": tareas[i].idVersion,
                    "NameFile": tareas[i].NameFile,
                    "Error": tareas[i].Error,
                    "FechaRegistro": tareas[i].FechaRegistro
                };
                tareasSend.push(tareaSend);
            }

            $.ajax({
                url: "/api/Cliente/"+tareasSend[0].idClientes+"/Version/"+tareasSend[0].idVersion+"/ReportarTodasTareas",
                type: "POST",
                dataType: 'text',
                data: JSON.stringify(tareasSend),
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudieron reportar las tareas');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudieron reportar las tareas');
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

        function reportarTareaAtrasada(tarea) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var tareaSend = {
                "idTareas": tarea.idTareas,
                "idClientes": tarea.idClientes,
                "Ambientes": {
                    "Nombre": tarea.Ambientes.Nombre
                },
                "Estado": tarea.Estado,
                "idVersion": tarea.idVersion,
                "NameFile": tarea.NameFile,
                "Error": tarea.Error,
                "FechaRegistro": tarea.FechaRegistro
            };

            $.ajax({
                url: "/api/ReportarTareaAtrasada",
                type: "POST",
                dataType: 'Json',
                data: tareaSend,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo reportar la tarea');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudo reportar la tarea');
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

        function detalleTareasNoEx(idCliente, idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/Version/' + idVersion + '/DetalleTareaAtrasada',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene tareas SQL');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene tareas SQL');
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

        function existeTareaAtrasada(idCliente, idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/Version/' + idVersion + '/TareaAtrasada',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene tareas SQL');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene tareas SQL');
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

        function ambienteOK(idAmbiente, idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Ambiente/'+idAmbiente+'/Version/'+idVersion+'/OK',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene componente SQL');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene componente SQL');
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

        function getScript(idVersion, NameFile) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Version/'+idVersion+'/Componentes/'+NameFile+'/script',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene componente SQL');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene componente SQL');
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

        function getVersiones(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Clientes/' + id + '/Versiones',
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
                    console.error('error = ' + xhr.status);
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

        function getCliente(id) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Usuarios/' + id + '/Cliente',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No tiene cliente asignado');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No tiene cliente asignado');
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
                    console.error('error = ' + xhr.status);
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

        function getAmbientes(idCliente, idVersion) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Cliente/' + idCliente + '/Version/' + idVersion + '/Ambiente',
                type: "GET",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existen ambientes');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
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

        function addVersion(id, idCliente, idAmbiente, estado) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var ambiente = {
                "idAmbientes": idAmbiente,
                "Estado": estado
            };
            console.log(JSON.stringify(ambiente));
            $.ajax({
                url: "api/Version/"+ id +"/Cliente/"+ idCliente +"/Ambiente",
                type: "POST",
                dataType: 'Json',
                data: ambiente,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 201) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la versión al cliente');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudo agregar la versión al cliente');
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

        function addTarea(idVersion, idCliente, idAmbientes, CodPrf, Modulo, NameFile) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            var tarea = {
                "idClientes": idCliente,
                "Ambientes": {
                    "idAmbientes": idAmbientes
                },
                "CodPrf": CodPrf,
                "Estado": 0,
                "Modulo": Modulo,
                "NameFile": NameFile,
                "Error": ""
            }
                
            $.ajax({
                url: "api/Version/" + idVersion + "/Tarea",
                type: "POST",
                dataType: 'Json',
                data: tarea,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No se pudo agregar la tarea');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No se pudo agregar la tarea');
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

        function existeTarea(idCliente, idAmbiente, idVersion, nameFile) {
            var deferred = $q.defer();
            var promise = deferred.promise;

            $.ajax({
                url: '/api/Tarea/' + idCliente + '/' + idAmbiente + '/' + idVersion + '/' + nameFile + '/Existe',
                type: "get",
                dataType: 'Json',
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status == 200) {
                        //console.log(JSON.stringify(data));
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject('No existe la tarea');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.error('error = ' + xhr.status);
                    deferred.reject('No existe la tarea');
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