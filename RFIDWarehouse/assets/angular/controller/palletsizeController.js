(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('palletsizeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.sizes = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initPalletSizeForm = function () {
            $scope.sizes = {};
            $scope.sizes.SIZE = "";
            $scope.sizes.ACTIVE = "";
        }

        $scope.savePalletSize = function () {
            if ($scope.sizes.SIZE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.sizes.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../palletsize/savePalletSize',
                data: $scope.sizes
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.initPalletSizeForm();

            })
        }

        $scope.updatePalletSize = function () {
            if ($scope.sizes.SIZE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.sizes.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePalletSize',
                data: $scope.sizes
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPalletSizes = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../palletsize/getPalletSize" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.sizes = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletSizeInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getPalletSizeInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.sizes = response.data;
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());