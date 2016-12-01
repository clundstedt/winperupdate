(function () {
    'use strict';

    angular
        .module('app')
        .controller('modulos', modulos);

    modulos.$inject = ['$scope', '$routeParams','serviceModulos','$timeout'];

    function modulos($scope, $routeParams, serviceModulos, $timeout) {
        $scope.title = 'modulos';
        $scope.formData = {};
        $scope.formComp = {};
        $scope.formTipoComp = {};
        $scope.idModulo = 0;
        $scope.usuario = $("#idToken").val();
        $scope.loading = false;

        $scope.tipoalert = "";
        $scope.msgalert = "";

        $scope.accion = "Crear";

        //Componentes
        $scope.componentes = [];
        $scope.tipocomponentes = [];
        $scope.CreandoComponentesModulos = false;


        activate();

        function activate() {


            if (!jQuery.isEmptyObject($routeParams)) {
                $scope.idModulo = $routeParams.idModulo;
                serviceModulos.getModulo($scope.idModulo).success(function (data) {
                    $scope.formData.nombre = data.NomModulo;
                    $scope.formData.descripcion = data.Descripcion;
                    $scope.formData.iscore = data.isCore;
                    $scope.formData.directorio = data.Directorio;
                    $scope.formData.estado = data.Estado;
                    $scope.accion = "Modificar";
                    serviceModulos.getComponentesModulo($scope.idModulo).success(function (data) {
                        $scope.componentes = data;
                    }).error(function (err) {
                        console.error(err);
                    });
                }).error(function (err) {
                    console.error(err);
                });
            }

            serviceModulos.getTipoComponentes().success(function (data) {
                $scope.tipocomponentes = data;
            }).error(function (err) {
                console.error(err);
            });

            $scope.CargarTipoComponentes = function () {
                serviceModulos.getTipoComponentes().success(function (data) {
                    $scope.tipocomponentes = data;
                }).error(function (err) {
                    console.error(err);
                });
            }
            
            $scope.CargarComponentesModulos = function () {
                serviceModulos.getComponentesModulo($scope.idModulo).success(function (data) {
                    $scope.componentes = data;
                }).error(function (err) {
                    console.error(err);
                });
            }
            
            $scope.CrearComponentesModulos = function (formComp) {
                serviceModulos.addComponentesModulos(formComp.nombre, formComp.descripcion, $scope.idModulo, formComp.tipocomponente).success(function (data) {
                    $scope.CargarComponentesModulos();
                    $("#mancom-modal").modal('toggle');
                }).error(function (err) {
                    console.error(err);
                });
            }

            $scope.ModificarComponentesModulos = function (componenteModulo) {
                componenteModulo.LblModificar = "Modificando...";
                serviceModulos.updComponentesModulos(componenteModulo.idComponentesModulos, componenteModulo.Nombre, componenteModulo.Descripcion, componenteModulo.TipoComponentes.idTipoComponentes).success(function (data) {
                    componenteModulo.LblModificar = "Modificado!";
                    $timeout(function () {
                        componenteModulo.LblModificar = "Modificar";
                    }, 3000);
                }).error(function (err) {
                    console.error(err);
                });
            }
            
            $scope.CrearTipoComponente = function (formTipoComp) {
                serviceModulos.addTipoComponentes(formTipoComp.nombre, formTipoComp.iscompbd).success(function (data) {
                    $scope.CargarTipoComponentes();
                }).error(function (err) {
                    console.error(err);
                });
            }

            $scope.EliminarTipoComponente = function (tipoComponente) {
                if (tipoComponente.LblEliminarTipo == "Eliminar") {
                    tipoComponente.LblEliminarTipo = "Confirme!";
                    $timeout(function () {
                        tipoComponente.LblEliminarTipo = "Eliminar";
                    }, 3000);
                } else {
                    serviceModulos.delTipoComponentes(tipoComponente.idTipoComponentes).success(function (data) {
                        $scope.CargarTipoComponentes();
                    }).error(function (err) {
                        console.error(err);
                    });
                }
            }

            $scope.EliminarComponentesModulos = function (componentesModulos) {
                if (componentesModulos.LblEliminar == "Eliminar") {
                    componentesModulos.LblEliminar = "Confirme!";
                    $timeout(function () {
                        componentesModulos.LblEliminar = "Eliminar";
                    }, 3000);
                } else {
                    serviceModulos.delComponentesModulos(componentesModulos.idComponentesModulos).success(function (data) {
                        $scope.CargarComponentesModulos();
                    }).error(function (err) {
                        console.error(err);
                    });
                }
            }


            $scope.Accion = function (formData) {
                if ($scope.accion == "Crear") {
                    $scope.Crear(formData);
                } else if ($scope.accion == "Modificar") {
                    $scope.Modificar(formData);
                }
            }

            $scope.Crear = function (formData) {
                $scope.loading = true;
                serviceModulos.addModulo(formData.nombre, formData.descripcion, formData.iscore, formData.directorio).success(function (data) {
                    $scope.accion = "Modificar";
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo creado exitosamente!.";
                    $scope.idModulo = data.idModulo;
                    $scope.formData.estado = data.Estado;
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de creación del módulo, vuelva a intentarlo.";
                });
                $scope.loading = false;
            }

            $scope.Modificar = function (formData) {
                $scope.loading = true;
                serviceModulos.updModulo($scope.idModulo, formData.nombre, formData.descripcion, formData.iscore, formData.directorio).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo modificado exitosamente!."
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert="Ocurrió un error durante el proceso de modificación, vuelva a intentarlo."
                });
                $scope.loading = false;
            }

            $scope.Eliminar = function () {
                $scope.loading = true;
                serviceModulos.delModulo($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo fue caducado con exito!.";
                    $scope.formData.estado = 'C';
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso de caducación del modulo, vuelva a intentarlo.";
                });
                $("#confelim-modal").modal('toggle');
                $scope.loading = false;
            }

            $scope.Vigente = function () {
                $scope.loading = true;
                serviceModulos.setVigente($scope.idModulo).success(function (data) {
                    $scope.tipoalert = "success";
                    $scope.msgalert = "Módulo establecido como vigente exitosamente!.";
                    $scope.formData.estado = 'V';
                }).error(function (err) {
                    console.error(err);
                    $scope.tipoalert = "danger";
                    $scope.msgalert = "Ocurrió un error durante el proceso, vuelva a intentarlo.";
                });
                $("#confvigen-modal").modal('toggle');
                $scope.loading = false;
            }

            $scope.ShowManTipCom = function () {
                $("#mancom-modal").modal('hide');
                $("#mantipcom-modal").modal({ backdrop: 'static', keyboard: false, show: true });
                $scope.CreandoComponentesModulos = true;
            }

            $scope.CreandoComponentes = function () {
                if ($scope.CreandoComponentesModulos) {
                    $("#mancom-modal").modal({ backdrop: 'static', keyboard: false, show: true });
                    $scope.CreandoComponentesModulos = false;
                }
                $("#mantipcom-modal").modal('hide');
            }
        }
    }
})();
