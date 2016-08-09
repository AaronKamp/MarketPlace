"use strict";
define(['angularAMD', 'marketPlaceService', 'searchCatalogueController', 'categorySwiper', 'isLoaded', 'configService'], function (angularAMD) {

    angularAMD.controller('MarketPlaceController', ['$scope', '$rootScope', 'marketPlaceService', '$location', '$routeParams', '$uibModal', function ($scope, $rootScope, marketPlaceService, $location, $routeParams, $uibModal, $uibModalInstance) {

        var path = $location.$$path;
       
        if (path != undefined && path.indexOf("UserAuth") > -1) {
            localStorage.setItem("user_data", $routeParams.userData);
            localStorage.setItem("jwt_token", $routeParams.jwtToken);
            localStorage.setItem("base_uri", $routeParams.apiBaseUri);
        }
       


        var self = this;
        $scope.activeTab = 0;
        var tabs = [{ title: 'Add-ons', index: 1 },
                    { title: 'Offers', index: 2 },
        ];
        self.addons = [];
        self.offers = [];
        var category1 = { id: '1', name: 'All' };
        var category2 = { id: '2', name: 'Newly Added' };
        var category3 = { id: '3', name: 'My Add-Ons' };
        var category3Offer = { id: '3', name: 'My Offers' };
        self.categoryList = [];
        var selectedCategory = 0;
        $scope.selectedCat = 0;
        $scope.noWrapSlides = false;
        self.addFixedTop = true;
        $scope.selectedTab = 1;
        $scope.SearchKey = '';
        $scope.searchBtnVisiblity = true;
        $scope.categoryName = 'All';
        $scope.isLoaded = false;
        if ($routeParams.type != undefined) {
            $scope.selectedTab = $routeParams.type;
            $scope.activeTab = $scope.selectedTab - 1;
        }
        if ($routeParams.searchKey != undefined) {
            $scope.SearchKey = $routeParams.searchKey;
        }
        if ($routeParams.categoryName != undefined) {
            $scope.categoryName = $routeParams.categoryName;
            if ($rootScope.selectedCatIndex != 0) {
                $scope.selectedCat = $rootScope.selectedCatIndex;
                selectedCategory = $rootScope.selectedCatIndex;
            }

        } else {
            $rootScope.selectedCatIndex = 0;
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

        $scope.imageList = [];
        //$scope.imageList.push({ id: 1, image: '/Images/002.png' });
        //$scope.imageList.push({ id: 2, image: '/Images/003.png' });
        //$scope.imageList.push({ id: 3, image: '/Images/004.jpg' }),
        //$scope.imageList.push({ id: 4, image: '/Images/005.jpg' });
        //$scope.imageList[0].active = true;
        $scope.myInterval = 3000;

        $scope.next = function ($index) {
            var i = $index;
            $scope.imageList[i++].active = true;
        };
        // Decrement carousel thing
        $scope.prev = function ($index) {
            var i = $index;
            $scope.imageList[i--].active = true;
        };

        $scope.tabs = tabs;
        $scope.isSet = function (tabNum) {
            return $scope.selectedTab == tabNum;
        };

        self.setTab = function (newTab) {
            $scope.selectedTab = newTab;
            $scope.categoryName = "All";
            angular.element(document.getElementById(selectedCategory)).removeClass("selectedCategory");
            angular.element(document.getElementById(0)).addClass("selectedCategory");
            selectedCategory = 0;
            loadAfresh();
        };

        self.modelOneData = {
            SearchKey: ""
        };

        var getMarketPlaceList = function (tab, searchKey, categoryName) {
            $scope.isLoaded = true;
            marketPlaceService.getMarketPlaceList(tab, searchKey, categoryName)
              .then(function (data) {
                  if (tab == 1) {
                      self.addons = data.services;
                  } else {
                      self.offers = data.services
                  }
              }).finally(function () {
                  // called no matter success or failure
                  $scope.isLoaded = false;
              });
        },

        getCategories = function (tab) {
            //$scope.isLoaded = true;

            //****************** Umcomment this if categories are fetched using API********
            //marketPlaceService.getCategories(tab)
            //  .then(function (data) {
            //      self.categoryList = data;
            //      if (tab == 1) {
            //          self.categoryList.unshift(category3);
            //      }
            //      if (tab == 2) {
            //          self.categoryList.unshift(category3Offer);
            //      }
            //      self.categoryList.unshift(category2);
            //      self.categoryList.unshift(category1);
            //  }).finally(function () {
            //      // called no matter success or failure
            //      //$scope.isLoaded = false;
            //  });
            //*****************************************************

            //****************** For the timebeing there are only 3 categories********

            if (tab == 1) {
                self.categoryList.unshift(category3);
            }
            if (tab == 2) {
                self.categoryList.unshift(category3Offer);
            }
            self.categoryList.unshift(category2);
            self.categoryList.unshift(category1);

            //****************************************************
        },
        getSlideShowImages = function (tab) {
            $scope.isLoaded = true;
            marketPlaceService.getSlideShowImages(tab)
              .then(function (data) {
                  $scope.imageList = data;
                  $scope.imageList[0].active = true;
              }).finally(function () {
                  // called no matter success or failure
                  $scope.isLoaded = false;
              });
        },

       loadAfresh = function () {
           getSlideShowImages($scope.selectedTab)
           getCategories($scope.selectedTab);
           getMarketPlaceList($scope.selectedTab, $scope.SearchKey, $scope.categoryName);
       },

        init = function () {
            loadAfresh();
        }();

        self.navigate = function (path) {
            $location.path(path);
        };

        self.openSearchModal = function () {
            $scope.searchBtnVisiblity = false;
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'ngViews/SearchModal.html',
                controller: 'SearchController',
                size: 'sm',
                windowClass: 'search-modal',
                resolve: {
                    modelOne: function () {
                        return self.modelOneData;
                    }
                }
            });

            modalInstance.result.then(function (SearchKey) {
                $scope.SearchKey = SearchKey;
                $scope.searchBtnVisiblity = true;
                if (SearchKey.length > 0) {
                    getMarketPlaceList($scope.selectedTab, $scope.SearchKey, $scope.categoryName);
                }

            }, function () {
                $scope.searchBtnVisiblity = true;
            });
        };

        self.getListOnCategory = function (category, index) {
            $scope.categoryName = category.name;
            angular.element(document.getElementById(selectedCategory)).removeClass("selectedCategory");
            angular.element(document.getElementById(index)).addClass("selectedCategory");
            selectedCategory = index;
            $rootScope.selectedCatIndex = index;
            $scope.SearchKey = '';
            getMarketPlaceList($scope.selectedTab, $scope.SearchKey, $scope.categoryName);
        };
    }]);
});