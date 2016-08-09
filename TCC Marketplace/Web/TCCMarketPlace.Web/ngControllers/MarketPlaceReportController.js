"use strict";
define(['angularAMD', 'marketPlaceService'], function (angularAMD) {

    angularAMD.controller('MarketPlaceReportController', ['$scope', 'marketPlaceService', '$location', '$window', '$routeParams', '$sce', 'notificationService', function ($scope, marketPlaceService, $location, $window, $routeParams, $sce, notificationService) {

        var self = this;
        self.Id = $routeParams.id;

        self.ReportUrl = { src: "" };

        if (self.Id != null || self.Id != undefined) {
            $scope.isLoaded = true;
            marketPlaceService.getServiceDetails(self.Id)
              .then(function (data) {
                  self.service = data;
                  if (self.isIframe()) {

                      if ($routeParams.reportUrl != null && $routeParams.reportUrl != '') {
                          self.service.reportUrl = encodeURIComponent($routeParams.reportUrl);
                          marketPlaceService.SaveReportUrl(self.service);
                          self.ViewReports();
                      }
                      else {
                          notificationService.notify("No report available.", { type: 'danger', autoClose: false, top: 45, left:45 });
                      }
                  }
                  else {
                      self.ViewReports();
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

        self.ViewReports = function () {

            if (self.service.reportUrl == null || self.service.reportUrl == "") {
                self.ReportUrl.src = $sce.trustAsResourceUrl(decodeURIComponent(self.service.signUpUrl) + '&returnUrl=' + $window.location.href);
                // in case using thirdparty mock page
                //self.ReportUrl.src = $sce.trustAsResourceUrl(decodeURIComponent(self.service.signUpUrl) + '&returnUrl=' + encodeURIComponent($window.location.href));

            }
            else {

                if (self.isIframe()) {

                    $scope.isLoaded = true;
                    $window.location.href = decodeURIComponent(self.service.reportUrl)
                    $scope.isLoaded = false;
                }
                else {
                    $scope.isLoaded = true;
                    self.ReportUrl.src = $sce.trustAsResourceUrl(decodeURIComponent(self.service.reportUrl));
                    $scope.isLoaded = false;
                }
            }
        };

        self.navigate = function (path) {
            if (self.Id != undefined) {
                path = path + '/' + self.Id;
            }

        };

        self.isIframe = function () {
            if ($window.location !== $window.parent.location)
                return true;
            else return false;
        };

    }]);

});