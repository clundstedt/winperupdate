(function () {
    'use strict';

    angular
        .module('app')
        .controller('admin', admin);

    admin.$inject = ['$scope', 'serviceAdmin'];

    function admin($scope, serviceAdmin) {
        $scope.title = 'admin';

        activate();

        function activate() {
            $scope.versiones = [];
           

            serviceAdmin.getVersiones().success(function (data) {
                $scope.versiones = data;
            }).
            error(function (data) {
                console.error(data);
            });

            $scope.GenerarVersionInicial = function () {
                $('#loading-modal').modal({ backdrop: 'static', keyboard: false })
                serviceAdmin.addVersionInicial('7.0.1', 'N', 'Versión inicial de WinPer').success(function (data) {
                    console.log(data);
                    serviceAdmin.GenVersionInicial(data.IdVersion).success(function (data1) {
                        serviceAdmin.getVersiones().success(function (data) {
                            $scope.versiones = data;
                            $('#loading-modal').modal('toggle');
                        }).error(function (data) {
                            console.error(data);
                        });
                    }).error(function (err1) {
                        console.error(err1);
                    });
                }).error(function (err) {
                    console.error(err);
                });
            }
        }
    }
})();
