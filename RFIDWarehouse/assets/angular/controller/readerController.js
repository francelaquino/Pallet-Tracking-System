(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('readerController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
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

        $scope.getGate = function () {
            $http({
                method: "GET",
                url: "/en/gate/getActiveGates" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.gates = response.data;

            })
        }

        $scope.getReaderTime = function () {
            $http({
                method: "GET",
                url: "/en/reader/getReaderTime" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.times = response.data;

            })
        }


        $scope.initReaderForm = function () {
            $scope.readers = {};
            $scope.readers.READERNAME = "";
            $scope.readers.LOCATION = "";
            $scope.readers.IPADDRESS = "";
            $scope.readers.PORT = "";
            $scope.readers.TIME = "";

            $scope.getGate();
            $scope.getReaderTime();


        }

        $scope.saveReader = function () {
            if ($scope.readers.MODEL == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
            if ($scope.readers.READERNAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.GATE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.IPADDRESS == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.PORT == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.readers.TIME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            $http({
                method: 'POST',
                url: '../reader/saveReader',
                data: $scope.readers
            })
            .success(function (response) {
                if (response == "") {
                    $scope.readers = {};
                    $(".alert-info").html("Reader successfully saved.");
                } else {
                    $(".alert-info").html(response);
                }
                $scope.toggleInfo();
                

            })
        }

        $scope.getReaders = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../reader/getReaders" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {


                $scope.readers = response.data;
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getReaderInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getReaderInfo/" + id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.readers = response.data;
                if ($scope.readers.PORT != "") {
                    $scope.readers.PORT = Number($scope.readers.PORT);
                }
                
                $("#progressWindow").modal("hide");

            })

        }

        $scope.updateReader = function () {
           
            if ($scope.readers.MODEL == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }
            if ($scope.readers.READERNAME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.GATE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.IPADDRESS == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.PORT == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.readers.TIME == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.readers.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields marked with (*)");
                $scope.toggleInfo();
                return false;
            }



            $http({
                method: 'POST',
                url: '../updateReader',
                data: $scope.readers
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
