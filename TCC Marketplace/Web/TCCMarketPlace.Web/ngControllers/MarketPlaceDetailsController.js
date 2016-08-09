"use strict";
define(['angularAMD', 'marketPlaceService'], function (angularAMD) {

    angularAMD.controller('MarketPlaceDetailsController', ['$scope', '$animate', 'marketPlaceService', '$location', '$routeParams', '$window', 'notificationService', function ($scope, $animate, marketPlaceService, $location, $routeParams, $window ,notificationService) {

        var self = this;
        self.Id = $routeParams.id;
        self.type = $routeParams.type!=undefined?$routeParams.type:"1";
        self.searchKey = $routeParams.searchKey;
        self.categoryName = $routeParams.categoryName;

        self.service;
       
        self.purchaseUrl = "";
        var serviceType = self.type == "1" ? "Add on" : "Offer";
        self.enableText = "Enable " + serviceType;
        self.removeText = "Remove " + serviceType;
        $scope.isLoaded = false;
        if (self.Id != null || self.Id != undefined) {
            $scope.isLoaded = true;
            marketPlaceService.getServiceDetails(self.Id)
              .then(function (data) {
                  self.service = data;
                  self.enableText = self.service.isEnabled ? "Disable " + serviceType : "Enable " + serviceType;
                        
                  if (self.service.price > 0 ) {
                      self.purchaseUrl = "/CreatePurchaseTransaction?productid=" + self.service.productId + "&serviceid=" + self.service.serviceId + "&thermostatid=" + self.service.thermostatId;
                  }

                 
              }).finally(function () {
                  $scope.isLoaded = false;
              });
        }
        var getMobileOperatingSystem = function () {
            var userAgent = navigator.userAgent || navigator.vendor || window.opera;

            if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
                return 'iOS';

            }
            else if (userAgent.match(/Android/i)) {

                return 'Android';
            }
            else {
                return 'unknown';
            }
        };

        $scope.buttonVisiblity = function () {

            if (getMobileOperatingSystem() == 'Android') {
                return false;
            } return true;

        };

        self.navigate = function (path) {

            if (self.type != undefined) {
                path = path + '/' + self.type;
            }
            if (self.categoryName != undefined) {
                path = path + '/' + self.categoryName;
            }
            if (self.searchKey != undefined) {
                path = path + '/' + self.searchKey;
            }

            $location.path(path);
        };
       
        self.EnableOrDisableService = function () {
            $scope.isLoaded = true;
            marketPlaceService.EnableOrDisableService(self.service)
            .then(function (data) {
                if (!data.hasError) {
                    self.enableText = data.data.isEnabled ? "Disable " + serviceType : "Enable " + serviceType;
                    notificationService.notify("Service " + (self.service.isEnabled == true ? "disabled" : "enabled") + " successfully", { type: 'success', timeout: 2000 });
                } else {
                    notificationService.notify(data.errorMessage, { type: 'danger',autoClose: false });
                }

            }, function (err) {
                //error callback                    
                notificationService.notify(err, { autoClose: false, type: "danger" });
            }).finally(function () {
                // called no matter success or failure
                $scope.isLoaded = false;
            });
        };

        self.RemoveService = function () {
            $scope.isLoaded = true;
            marketPlaceService.RemoveService(self.service)
            .then(function (data) {
                if (!data.hasError) {
                    self.service.isBought = false;
                    notificationService.notify(serviceType + " removed successfully", { type: 'success', timeout: 1000 }).then(function () {
                        $location.path('/');
                        $scope.$apply();
                    });
                } else {
                    notificationService.notify(data.errorMessage, { type: 'danger', autoClose: false });
                }
            }, function (err) {
                //error callback                    
                notificationService.notify(err, { autoClose: false, type: "danger" });
            }).finally(function () {
                // called no matter success or failure
                $scope.isLoaded = false;
            });
        };

        self.SubscribeToService = function () {
            if (self.purchaseUrl.length > 0) {
                return false;
            }
            $scope.isLoaded = true;
            marketPlaceService.SubscribeToService(self.service)
            .then(function (data) {
                if (!data.hasError) {
                    self.service.isBought = true;
                    if (data.data == "Service already subscribed")
                        notificationService.notify(data.data, { type: 'success', timeout: 1000 }).then(function () {
                            $location.path("MarketPlaceReports/" + self.service.serviceId);
                            $scope.$apply();
                        });
                    else
                        notificationService.notify(serviceType + " subscribed successfully", { type: 'success', timeout: 1000 }).then(function () {
                            $location.path("MarketPlaceReports/" + self.service.serviceId);
                            $scope.$apply();
                    });

                } else {
                    notificationService.notify(data.errorMessage, { type: 'danger', autoClose: false });
                }
            }, function (err) {
                //error callback                    
                notificationService.notify(err, { autoClose: false, type: "danger" });
            }).finally(function () {
                // called no matter success or failure
                $scope.isLoaded = false;
            });
        };

        self.PurchaseService = function () {
            if (self.purchaseUrl.length == 0) {
                return false;
            }
            $scope.isLoaded = true;
            marketPlaceService.getServiceSubscriptionStatus(self.service.serviceId)
            .then(function (data) {
                if (!data.hasError) {
                    self.service.isBought = data;
                    if (!self.service.isBought) {
                        $window.location.href = self.purchaseUrl;
                    }
                    else {
                        notificationService.notify(serviceType + " already subscribed", { type: 'success', timeout: 1000 }).then(function () {
                        });
                    }
                }
            }, function (err) {
                //error callback                    
                notificationService.notify(err, { autoClose: false, type: "danger" });
            }).finally(function () {
                // called no matter success or failure
                $scope.isLoaded = false;
            });

        };


    }]);
});