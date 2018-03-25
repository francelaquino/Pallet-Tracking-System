(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('palletlocationController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.locations = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initPalletLocationForm = function () {
            $scope.locations = {};
            $scope.locations.LOCATION = "";
            $scope.locations.ACTIVE = "";
        }

        $scope.savePalletLocation = function () {
            if ($scope.locations.LOCATION == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.locations.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../palletlocation/savePalletLocation',
                data: $scope.locations
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.initPalletLocationForm();

            })
        }

        $scope.updatePalletLocation = function () {
            if ($scope.locations.LOCATION == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.locations.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePalletLocation',
                data: $scope.locations
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPalletLocations = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../palletlocation/getPalletLocation" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.locations = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletLocationInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getPalletLocationInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.locations = response.data;
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());