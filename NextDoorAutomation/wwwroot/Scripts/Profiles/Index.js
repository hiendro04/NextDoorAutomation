app.controller('homeIndexNewCtrl', function ($scope, $mdDialog, $http, $timeout, $templateCache) {
    $scope.message = "Tool đăng ký thi tiếng";

    $scope.hide = function () {
        $mdDialog.hide();
    };
    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.getData = function () {
        $http.post("/Profile/GetDataNeighborhood", reqData)
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