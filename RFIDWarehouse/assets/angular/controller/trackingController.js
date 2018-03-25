(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('trackingController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.readers = {};
        $scope.RFID = {};
        $scope.search = {};

    
      

        $scope.getAreas = function () {
            $http({
                method: "GET",
                url: "/en/area/getActiveArea" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.areas = response.data;

            })
            $scope.getPalletCount();
        }

        $scope.getPalletCount = function () {
            $("#progressWindow").modal("show");
            $(".datatable").hide();
            $('#datatable').dataTable().fnDestroy();
            $http({
                method: "GET",
                url: "/en/tracking/getPalletCount" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.pallets = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable({ "searching": false, "ordering": false, });
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletActivity = function () {
            $("#progressWindow").modal("show");
            $(".datatable").hide();
            $('#datatable').dataTable().fnDestroy();
            $http({
                method: "GET",
                url: "/en/tracking/getPalletActivity/"+$scope.search.DAY + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.pallets = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable({ "searching": false, "ordering": false, });
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }


        $scope.getPalletMovements = function () {
            if ($scope.search.AREA != "") {
                $("#progressWindow").modal("show");
                $(".datatable").hide();
                $('#datatable').dataTable().fnDestroy();
                $http({
                    method: "GET",
                    url: "/en/tracking/getPalletMovements/" + $scope.search.AREA + "?rnd=" + new Date().getTime(),
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

        $scope.getPalletLocation = function () {
            if ($scope.search.AREA != "") {
                $(".datatable").hide();
                $('#datatable').dataTable().fnDestroy();
                $("#progressWindow").modal("show");
                $http({
                    method: "GET",
                    url: "/en/tracking/getPalletLocation/" + $scope.search.AREA + "?rnd=" + new Date().getTime(),
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

        $scope.getPalletInvetory = function () {
            $http({
                method: "GET",
                url: "/en/tracking/getAreaTypeInventory"+ "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.types = response.data;
                setTimeout(function () {
                    $('#inventory').dataTable({ "searching": false, "ordering": false,"bPaginate":false,"bInfo":false });
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);


            })

            $http({
                method: "GET",
                url: "/en/tracking/getAreaInventory" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.areas = response.data;
                setTimeout(function () {
                    $('#area').dataTable({ "searching": false, "ordering": false, "bPaginate": false, "bInfo": false });
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);


            })
        }

       



    }]);


}());
