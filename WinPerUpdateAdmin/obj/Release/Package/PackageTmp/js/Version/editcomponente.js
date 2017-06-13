(function () {
    'use strict';

    angular
        .module('app')
        .controller('editcomponente', editcomponente);

    editcomponente.$inject = ['$scope', '$routeParams', '$window', 'serviceAdmin', '$timeout'];

    function editcomponente($scope, $routeParams, $window, serviceAdmin, $timeout) {
        $scope.title = 'editcomponente';

        activate();

        function activate() {
            $scope.msgError = "";

            console.log(JSON.stringify($routeParams));
            $scope.idVersion = $routeParams.idVersion;
            console.log($scope.idVersion);
            $scope.namecomponente = $routeParams.name;
            $scope.modulos = [];
            $scope.formData = {};
            $scope.componentes = [];

            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            serviceAdmin.getModulos().success(function (data) {
                $scope.modulos = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });

            
            serviceAdmin.getComponente($scope.idVersion, $routeParams.name).success(function (data) {
                $scope.formData.modulo = data.Modulo;
                $scope.formData.comentario = data.Comentario;
                console.log(data.Modulo);
                console.log(data.Comentario);
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });


            serviceAdmin.getComponentesByName($routeParams.name).success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    if(data[i].idVersion != $scope.idVersion){
                        $scope.componentes.push(data[i]);
                    }
                }
                console.log($scope.componentes);
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
            });
            

            $scope.titulo = 'Editar Componente';
            $scope.labelcreate = 'Guardar';
            $scope.increate = true;

            $scope.SaveVersion = function (formData) {
                $scope.labelcreate = 'Guardando';
                $scope.increate = false;
                console.log(JSON.stringify($scope.formData));
                serviceAdmin.updComponente($scope.idVersion, $scope.namecomponente, formData.modulo, formData.comentario).success(function () {
                    $scope.labelcreate = 'Guardar';
                    $scope.increate = true;
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }

            $scope.ShowConfirm = function () {
                $("#delete-modal").modal('show');
            }

            $scope.Eliminar = function () {
                serviceAdmin.delComponente($scope.idVersion, $scope.namecomponente).success(function () {
                    $('.close').click();

                    $window.setTimeout(function () {
                        $window.location.href = "/Admin#/EditVersion/" + $scope.idVersion;
                    }, 2000);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";
                });
            }
        }
    }
})();
