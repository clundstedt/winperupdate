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
            $scope.msgSuccess = "";

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
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            serviceAdmin.getModulos().success(function (data) {
                $scope.modulos = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            
            serviceAdmin.getComponente($scope.idVersion, $routeParams.name).success(function (data) {
                $scope.formData.modulo = data.Modulo;
                $scope.formData.comentario = data.Comentario;
                console.log(data.Modulo);
                console.log(data.Comentario);
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            /*carga el historial de cambios de un componente
            serviceAdmin.getComponentesByName($routeParams.name).success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    if(data[i].idVersion != $scope.idVersion){
                        $scope.componentes.push(data[i]);
                    }
                }
                console.log($scope.componentes);
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });
            */

            $scope.titulo = 'Editar Componente';
            $scope.labelcreate = 'Modificar';
            $scope.increate = true;

            $scope.SaveVersion = function (formData) {
                $scope.msgSuccess = "";
                $scope.labelcreate = 'Modificando';
                $scope.increate = false;
                console.log(JSON.stringify($scope.formData));
                serviceAdmin.updComponente($scope.idVersion, $scope.namecomponente, formData.modulo, formData.comentario).success(function () {
                    $scope.labelcreate = 'Modificar';
                    $scope.increate = true;
                    $scope.msgError = "";
                    $scope.msgSuccess = "Componente modificado exitosamente!.";
                    window.scrollTo(0,0);
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
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
                    }, 3000);
                    $scope.msgError = "";
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }
        }
    }
})();
