app.controller('homeIndexNewCtrl', function ($scope, $mdDialog, $http, $timeout, $templateCache) {
    $scope.message = "Tool đăng ký thi tiếng";

    $scope.hide = function () {
        $mdDialog.hide();
    };
    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.GetDataCity = function () {
        $http.post("/Profile/GetDataCity")
            .then(function (rs) {
                if (rs.data.returnCode == "1") {
                    $scope.hide();
                }
                else {
                    $scope.cancel();
                }
            });
    }

    $scope.GetDataNeighborhood = function () {
        $http.post("/Profile/GetDataNeighborhood")
            .then(function (rs) {
                if (rs.data.returnCode == "1") {
                    $scope.hide();
                }
                else {
                    $scope.cancel();
                }
            });
    }


    $scope.init = function () {
    }
    $scope.init();
});