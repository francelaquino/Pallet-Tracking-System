(function () {
    'use strict';

    var app = angular.module('RFID',[]);

    app.controller('pallettypeController', ['$scope', '$http','$location', function ($scope, $http,$location) {
        $scope.types = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initPalletTypeForm = function () {
            $scope.types = {};
            $scope.types.TYPE = "";
            $scope.types.ACTIVE = "";
        }

        $scope.savePalletType = function () {
            if ($scope.types.TYPE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.types.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../pallettype/savePalletType',
                data: $scope.types
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.initPalletTypeForm();

            })
        }

        $scope.updatePalletType = function () {
            if ($scope.types.TYPE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.types.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePalletType',
                data: $scope.types
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPalletTypes = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../pallettype/getPalletTypes" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.types = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletTypeInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getPalletTypeInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.types = response.data;
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());