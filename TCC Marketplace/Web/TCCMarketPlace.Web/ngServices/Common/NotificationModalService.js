"use strict";
define(['angularAMD'],function (angularAMD) {

   
    angularAMD.service('notificationModalService', ['$uibModal', function ($uibModal) {
        this.open = function (config) {

            var header = config.header || "Notification",
                message = config.message;

            var modalInstanceMessage = $uibModal.open({
                animation: true,
                template: '<div class="modal-header"><h4>' + header + '</h4></div><div class="modal-body">' + message + '</div><div class="modal-footer"> <button class="btn btn-primary" type="button" data-ng-click="cancel()">Ok</button></div>',
                controller: ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };

                }]
            });

            return modalInstanceMessage;
        }

    }]);

});