(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('reportController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.readers = {};
        $scope.RFID = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }

        $scope.getPalletLocation = function () {
            $http({
                method: "GET",
                url: "/en/palletlocation/getActivePalletLocation" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.locations = response.data;

            })
        }

        $scope.searchRFIDLocation = function () {
            if ($scope.search.LOCATION != "") {
                $("#progressWindow").modal("show");
                $http({
                    method: "GET",
                    url: "/en/report/searchRFIDLocation/" + $scope.search.LOCATION + "?rnd=" + new Date().getTime(),
                }).then(function (response) {
                    $scope.pallets = response.data;
                    setTimeout(function () {
                        $('#datatable').dataTable({ "searching": false, "ordering": false, });
                        $(".datatable").show();
                        $("#progressWindow").modal("hide");
                    }, 1000);


                })
            }
        }

       



    }]);


}());
