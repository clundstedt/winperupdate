(function () {
    'use strict';

    angular
        .module('app')
        .controller('editcomponente', editcomponente);

    editcomponente.$inject = ['$scope', '$routeParams', '$window', 'serviceAdmin'];

    function editcomponente($scope, $routeParams, $window, serviceAdmin) {
        $scope.title = 'editcomponente';

        activate();

        function activate() {
            console.log(JSON.stringify($routeParams));
            $scope.idVersion = $routeParams.idVersion;
            $scope.namecomponente = $routeParams.name;
            $scope.modulos = [];
            $scope.formData = {};

            serviceAdmin.getVersion($scope.idVersion).success(function (data) {
                $scope.version = data;
            }).error(function (data) {
                console.debug(data);
            });

            serviceAdmin.getModulos().success(function (data) {
                $scope.modulos = data;
            }).error(function (data) {
                console.debug(data);
            });

            serviceAdmin.getComponente($scope.idVersion, $routeParams.name).success(function (data) {
                $scope.formData.modulo = data.Modulo;
                $scope.formData.comentario = data.Comentario;

                console.log(JSON.stringify($scope.formData));
            }).error(function (data) {

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
                }).error(function (data) {
                    console.debug(data);
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

                }).error(function (data) {
                    console.debug(data);
                });
            }
        }
    }
})();
