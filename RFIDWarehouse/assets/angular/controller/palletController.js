(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('palletController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.pallets = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.getPalletType = function () {
            $http({
                method: "GET",
                url: "/en/pallettype/getActivePalletType"+"?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.pallettypes = response.data;

            })
        }

        $scope.getPalletSize = function () {
            $http({
                method: "GET",
                url: "/en/palletsize/getActivePalletSize" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.palletsizes = response.data;

            })
        }

        $scope.getPalletStyle = function () {
            $http({
                method: "GET",
                url: "/en/palletstyle/getActivePalletStyle" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.palletstyle = response.data;

            })
        }

        $scope.getPalletLocation = function () {
            $http({
                method: "GET",
                url: "/en/palletlocation/getActivePalletLocation" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.locations = response.data;

            })
        }

        $scope.getArea = function () {
            $http({
                method: "GET",
                url: "/en/area/getActiveArea" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.areas = response.data;

            })
        }

        $scope.getPalletSupplier = function () {
            $http({
                method: "GET",
                url: "/en/palletsupplier/getActivePalletSupplier" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.suppliers = response.data;

            })
        }

        $scope.initPalletForm = function () {
            $scope.pallets = {};
            $scope.pallets.RFID = "";
            $scope.pallets.BARCODE = "";
            $scope.pallets.TYPE = "";
            $scope.pallets.STYLE = "";
            $scope.pallets.SIZE = "";
            $scope.pallets.LOCATION = "";
            $scope.pallets.SUPPLIER = "";
            $scope.pallets.ACTIVE = "";

            $scope.getPalletType();
            $scope.getPalletStyle();
            $scope.getPalletSize();
            $scope.getPalletLocation();
            $scope.getPalletSupplier();
            $scope.getArea();

        }


        $scope.savePallet = function () {
            if ($scope.pallets.RFID == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.NAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.pallets.AREA == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

           

            if ($scope.pallets.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            $http({
                method: 'POST',
                url: '../pallet/savePallet',
                data: $scope.pallets
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPallets = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../pallet/getPallets" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                
                $scope.pallets = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }
       
        $scope.getPalletInfo = function () {
            var id = $location.search().id;
            var gid = $location.search().gid;

            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "/en/pallet/getPalletInfo/" + id +"/"+gid+ "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.pallets = response.data;
                console.log($scope.pallets);
                $("#progressWindow").modal("hide");

            })
        }


        $scope.initAssignPalletForm = function () {

            $http({
                method: "GET",
                url: "/en/employee/getActiveEmployees/"+ "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.employees = response.data;
                console.log($scope.employees);
                $("#progressWindow").modal("hide");

            })

            $http({
                method: "GET",
                url: "/en/pallet/getActivePallets/" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.pallet = response.data;
                $("#progressWindow").modal("hide");

            })

            
        }

        $scope.updatePallet = function () {
            if ($scope.pallets.RFID == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.NAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.LOCATION == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.SUPPLIER == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePallet',
                data: $scope.pallets
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }


        $scope.assignPallet = function () {
            console.log($scope.pallets);
            if ($scope.pallets.EMPLOYEE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.pallets.ID == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../pallet/assignPallet',
                data: $scope.pallets
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }


    }]);


}());