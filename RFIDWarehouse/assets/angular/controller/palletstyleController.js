(function () {
    'use strict';

    var app = angular.module('RFID');

    app.controller('palletstyleController', ['$scope', '$http','$location', function ($scope, $http,$location) {
        $scope.styles = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initPalletStyleForm = function () {
            $scope.styles = {};
            $scope.styles.STYLE = "";
            $scope.styles.ACTIVE = "";
        }

        $scope.savePalletStyle = function () {
           
            if ($scope.styles.STYLE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.styles.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../palletstyle/savePalletStyle',
                data: $scope.styles
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.initPalletStyleForm();

            })
        }

        $scope.updatePalletStyle = function () {
            if ($scope.styles.STYLE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.styles.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePalletStyle',
                data: $scope.styles
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPalletStyles = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../palletstyle/getPalletStyles" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.styles = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletStyleInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getPalletStyleInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.styles = response.data;
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());