"use strict";
define(['angularAMD','noSpecialCharacters'], function (angularAMD) {
    angularAMD.controller('SearchController', ['$scope', '$uibModalInstance', 'modelOne', function ($scope,  $uibModalInstance, modelOne) {

        $scope.SearchKey = '';
        $scope.search = function () {
            $uibModalInstance.close($scope.SearchKey);
        };
        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }]);


});