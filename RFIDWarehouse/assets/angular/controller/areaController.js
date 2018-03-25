(function () {
    'use strict';

    var app = angular.module('RFID');


    app.controller('areaController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.areas = {};

    
        $scope.toggleInfo = function () {
            $(".alert-info").show();
            if ($('.alert-info').is(":visible")) {
                setTimeout(function () {
                    $(".alert-info").hide();
                }, 8000);
            }
        }

        $scope.getAreaType = function () {
            $http({
                method: "GET",
                url: "/en/area/getAreaType" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.areatypes = response.data;

            })
        }
        $scope.viewMap = function ($id,$map) {
            $('.modal-title').text($id);
            $("#map").attr("src", "/Map/" + $map);
            $("#mapModal").modal('show');

        }

        $scope.initAreaForm = function () {
            $scope.areas = {};
            $scope.areas.AREA = "";
            $scope.areas.AREATYPE = "";
            $scope.areas.ACTIVE = "";
            $("#fileupload").val("");
            $scope.getAreaType();

        }

        $scope.saveArea = function () {
           

            if ($scope.areas.AREA == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.areas.AREATYPE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.areas.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }
           

            $http({
                method: 'POST',
                url: '../area/saveArea',
                data: $scope.areas
            })
            .success(function (response) {
                var id = response;
                if (id.length <= 10) {

                    var file = $('#fileupload').get(0).files[0];
                    if (file) {
                        var formData = new FormData();
                        formData.append("txtid", id);
                        formData.append("file", file);
                        $.ajax({
                            url: '../area/uploadMap',
                            type: 'POST',
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function (data) {


                            }
                        });
                    }
                    $scope.initAreaForm();
                    $(".alert-info").html("Area successfully saved");
                } else {
                    $(".alert-info").html(response);
                }
                $scope.toggleInfo();
                

            })
        }

        $scope.updateArea = function () {
            if ($scope.areas.AREA == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }

            if ($scope.areas.AREATYPE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            if ($scope.areas.ACTIVE == "") {
                $(".alert-info").html("Please fill required fields");
                $scope.toggleInfo();
                return false;
            }


            $http({
                method: 'POST',
                url: '../updateArea',
                data: $scope.areas
            })
            .success(function (response) {

                    var file = $('#fileupload').get(0).files[0];
                    if (file) {
                        var formData = new FormData();
                        formData.append("txtid", $scope.areas.ID);
                        formData.append("file", file);
                        $.ajax({
                            url: '../uploadMap',
                            type: 'POST',
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function (data) {


                            }
                        });
                    }
                $(".alert-info").html(response);
                $scope.toggleInfo();

            })
        }

        $scope.getAreas = function () {
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../area/getAreas" + "?rnd=" + new Date().getTime(),
            }).then(function (response) {

                $scope.areas = response.data;
                console.log($scope.areas);
                setTimeout(function () {
                    $('#datatable').dataTable();
                    $(".datatable").show();
                    $("#progressWindow").modal("hide");
                }, 1000);

            })
        }

        $scope.getAreaInfo = function () {
            var id = $location.search().id;
            $("#progressWindow").modal("show");
            $http({
                method: "GET",
                url: "../getAreaInfo/"+id + "?rnd=" + new Date().getTime(),
            }).then(function (response) {
                $scope.areas = response.data;
                console.log($scope.areas);
                    $("#progressWindow").modal("hide");

            })

        }

       


    }]);


}());