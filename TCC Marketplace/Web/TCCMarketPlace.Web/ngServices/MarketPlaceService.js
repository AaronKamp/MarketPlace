"use strict";
define(['angularAMD', ], function (angularAMD) {


    angularAMD.service('marketPlaceService', ['configService', '$http', '$log', '$q', function (configService, $http, $log, $q) {

        var baseUrl = configService.getApiBaseUrl() + "MarketPlace/",
        formUrlForGetList = function (tab, searchKey, categoryname) {
            var url = baseUrl;
            if (categoryname == "All") {
                url = url + "GetMarketPlaceList/" + tab + "?";
            }
            else if (categoryname == "Newly Added") {
                url = url + "GetNewlyAddedServices/" + tab + "?";
            }
            else if (categoryname == "My Add-Ons" || categoryname == "My Offers") {
                url = url + "GetMyServices/" + tab + "?";
            }
            else {
                url = url + "GetMarketPlaceList/" + tab + "?";
            }

            if (searchKey.length > 0) {
                url = url + "SearchKey=" + searchKey;
            }
            return url;
        },        
        formUrlForGetCategories = function (tab) {

            var url = baseUrl + "GetCategories/" + tab;
            return url;
        },
        formUrlForGetImages = function (tab) {

            var url = baseUrl + "GetSlideShowImages/" + tab;
            return url;
        },
        formUrlForGetDetails = function (id) {

            var url = baseUrl + "GetDetails/" + id  ;
            return url;
        },
        formUrlForSubscriptionStatus = function (id) {

            var url = baseUrl + "IsSubscribed/" + id;
            return url;
        };
        
        this.getMarketPlaceList = function (tab,searchKey,categoryname) {
            var deferred = $q.defer();
           
            $http.get(formUrlForGetList(tab, searchKey, categoryname))
               .success(function (data) {
                   deferred.resolve(data.data);
               }).error(function (msg, code) {
                   deferred.reject(msg);
               });
            return deferred.promise;
        };

        this.getCategories = function (tab) {
            var deferred = $q.defer();
            $http.get(formUrlForGetCategories(tab))
               .success(function (data) {
                   deferred.resolve(data.data);
               }).error(function (msg, code) {
                   deferred.reject(msg);
               });
            return deferred.promise;
        };

        this.getServiceDetails = function (id) {
            var deferred = $q.defer();
            $http.get(formUrlForGetDetails(id))
               .success(function (data) {
                   deferred.resolve(data.data);
               }).error(function (msg, code) {
                   deferred.reject(msg);
               });
            return deferred.promise;
        };

        this.getSlideShowImages = function (tab) {
            var deferred = $q.defer();
            $http.get(formUrlForGetImages(tab))
               .success(function (data) {
                   deferred.resolve(data.data);
               }).error(function (msg, code) {
                   deferred.reject(msg);
               });
            return deferred.promise;
        };

        this.getServiceSubscriptionStatus = function (id) {
            var deferred = $q.defer();
            $http.get(formUrlForSubscriptionStatus(id))
               .success(function (data) {
                   deferred.resolve(data.data);
               }).error(function (msg, code) {
                   deferred.reject(msg);
               });
            return deferred.promise;
        };

        this.EnableOrDisableService = function (service) {
            var deferred = $q.defer();
            var url = baseUrl + "EnableOrDisableService";
            $http.post(url, service)
              .success(function (data) {
                  deferred.resolve(data);
              }).error(function (msg, code) {
                  deferred.reject(msg);

              });
            return deferred.promise;
        };

        this.RemoveService = function (service) {
            var deferred = $q.defer();
            var url = baseUrl + "RemoveService";
            $http.post(url, service)
              .success(function (data) {
                  deferred.resolve(data);
              }).error(function (msg, code) {
                  deferred.reject(msg);

              });
            return deferred.promise;
        };


        this.SubscribeToService = function (service) {
            var deferred = $q.defer();
            var url = baseUrl + "SubscribeToService";
            $http.post(url, service)
              .success(function (data) {
                  deferred.resolve(data);
              }).error(function (msg, code) {
                  deferred.reject(msg);

              });
            return deferred.promise;
        };
        
        this.SaveReportUrl = function (service) {
            var deferred = $q.defer();
            var url = baseUrl + "SaveReportUrl";
            $http.post(url, service).success(function (data) {
                deferred.resolve(data);
            }).error(function (msg, code) {
                deferred.reject(msg);
            });
        };


    }]);
});