var app = angular.module("app", ["ngMaterial"]);

app.controller("mainCtrl", ["$scope", "$mdDialog", "$http", "$timeout", function ($scope, $mdDialog, $http, $timeout) {
    //declared variables
    let loggedIn = false;

    //declared functions
    let checkIfLoggedIn = function() {
        $http.get("/Login").then(function onSuccess(data) {
            if (data.data) {
                loggedIn = true;
            } else {
                showLoginDialog();
            }
        }, function onFailure(data) {
            console.log(data);
        });
    };

    let showLoginDialog = function() {
        $mdDialog.show({
            controller: "loginController",
            templateUrl: 'Template/LoginDialog',
            parent: angular.element(document.body),
            clickOutsideToClose: false
        })
    }

    let start = function() {
        $http.post("/Core/Start").then(function onSuccess(data) {
            console.log("success");
            $timeout(refreshStatus, 10000);
        }, function onFailure(data) {
            console.log(data);
        });
    }

    let refreshStatus = function() {
        $http.get("/Core/Updates").then(function onSuccess(data) {
            $scope.data = data.data;
        }, function onFailure(data) {
            console.log(data)
        });
        $timeout(refreshStatus, 3000);
    }

    //set scope variables
    $scope.start = start;

    checkIfLoggedIn();
}]);

app.controller("loginController", ["$scope", "$mdDialog", "$http", function($scope, $mdDialog, $http) {
        $scope.username = "";
        $scope.password = "";
        $scope.message = "";
        $scope.showProgress = false;

        $scope.login = function() {
            $scope.showProgress = true;
            let payload = {
                username: $scope.username,
                password: $scope.password
            };
            $http.post("/Login", payload).then(function onSuccess(data) {
                $scope.showProgress = false;
                $mdDialog.hide();
            }, function onFailure(data) {
                $scope.showProgress = false;
                $scope.message = data.data.ExceptionMessage;//but why
            });
        }
}]);