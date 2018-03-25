(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('employeeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.employees = {};


        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }


        $scope.initEmployeeForm = function () {
            $scope.employees = {};
            $scope.employees.EMAILADDRESS = "";
            $scope.employees.ADDRESS = "";


        }

        $scope.saveEmployee = function () {
            if ($scope.employees.EMPLOYEENO == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
            if ($scope.employees.NAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.employees.MOBILENO == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.employees.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            $http({
                method: 'POST',
                url: '../employee/saveEmployee',
                data: $scope.employees
            })
            .success(function (response) {
                if (response == "") {
                    $scope.readers = {};
                    $(".alert-info").html("Employee successfully saved.");
                } else {
                    $(".alert-info").html(response);
                }
                $scope.toggleInfo();
                

            })
        }

        $scope.getEmployees = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../employee/getEmployees" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {


                $scope.employees = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getEmployeeInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getEmployeeInfo/" + id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.employees = response.data;
                
                $("#progressWindow").modal("hide");

            })

        }

        $scope.updateEmployee = function () {
           
            if ($scope.employees.EMPLOYEENO == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
            if ($scope.employees.NAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.employees.MOBILENO == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.employees.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updateEmployee',
                data: $scope.employees
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }


        $scope.saveTag = function () {
           
            $http({
                method: 'POST',
                url: '../reader/saveTag',
                data: $scope.RFID
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();
                $scope.RFID.ID = "";
                $scope.RFID.TAG = "";

            })
        }



    }]);


}());
