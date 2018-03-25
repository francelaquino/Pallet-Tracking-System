(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('palletsupplierController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.suppliers = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initPalletSupplierForm = function () {
            $scope.suppliers = {};
            $scope.suppliers.SUPPLIER = "";
            $scope.suppliers.ACTIVE = "";
        }

        $scope.savePalletSupplier = function () {
            if ($scope.suppliers.SUPPLIER == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.suppliers.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../palletsupplier/savePalletSupplier',
                data: $scope.suppliers
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.initPalletSupplierForm();

            })
        }

        $scope.updatePalletSupplier = function () {
            if ($scope.suppliers.SUPPLIER == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.suppliers.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updatePalletSupplier',
                data: $scope.suppliers
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getPalletSuppliers = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../palletsupplier/getPalletSupplier" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.suppliers = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getPalletSupplierInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getPalletSupplierInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.suppliers = response.data;
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());