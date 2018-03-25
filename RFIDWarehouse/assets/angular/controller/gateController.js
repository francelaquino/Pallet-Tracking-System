(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('gateController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.gates = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();

                }, 8000);
            }
        }

       


        $scope.initGateForm = function () {
            $scope.gates = {};

        }

        $scope.saveGate = function () {
            if ($scope.gates.GATE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
          


            if ($scope.gates.MODE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.gates.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            $http({
                method: 'POST',
                url: '../gate/saveGate',
                data: $scope.gates
            })
            .success(function (response) {
                if (response == "") {
                    $scope.gates = {};
                    $(".alert-info").html("Gate successfully saved.");
                } else {
                    $(".alert-info").html(response);
                }
                $scope.toggleInfo();
                

            })
        }

        $scope.getGates = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../gate/getGates" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {


                $scope.gates = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getGateInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getGateInfo/" + id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.gates = response.data;
                
                $("#progressWindow").modal("hide");

            })

        }

        $scope.updateGate = function () {
           
            if ($scope.gates.GATE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
           


            if ($scope.gates.MODE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.gates.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }



            $http({
                method: 'POST',
                url: '../updateGate',
                data: $scope.gates
            })
            .success(function (response) {
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }




    }]);


}());
