(function () {
    'use strict';

    angular
        .module('app')
        .controller('inicio', inicio);

    inicio.$inject = ['$scope', 'serviceClientes', '$window'];

    function inicio($scope, serviceClientes, $window) {
        $scope.title = 'inicio';

        activate();

        function activate() {
            $scope.msgError = "";

            $scope.clientes = [];
            $scope.all = false;
            $scope.clienteNoVigente = null;


            serviceClientes.getClientes().success(function (data) {
                $scope.clientes = data;
                $scope.msgError = "";
            }).error(function (err) {
                console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
            });

            $scope.GenerarPDF = function () {
                //window.location = '/api/Clientes/PDF';
                console.log('llamamos a getClientesPDF()');
                serviceClientes.getClientesPDF().success(function (fileURL) {
                    console.log('OK PDF: ' + fileURL);
                    //$window.open(fileURL);
                    window.location = fileURL;
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio."; window.scrollTo(0, 0);
                })
            }

            $scope.ShowMdlMotivo = function (id) {
                serviceClientes.getClienteNoVigente(id).success(function (data) {
                    $scope.clienteNoVigente = data;
                    $("#mdlMotivo").modal('show');
                }).error(function (err) {
                    console.error(err); $scope.msgError = "Ocurrió un error durante la petición, contacte al administrador del sitio.";window.scrollTo(0,0);
                });
            }

        }
    }
})();
