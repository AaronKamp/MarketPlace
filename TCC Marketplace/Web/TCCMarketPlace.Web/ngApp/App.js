
'use strict';
define(['angularAMD', 'angularRoute', 'configService', 'uiBootstrap', 'uiBootstrapTpl', 'angularTouch', 'notificationService', 'interceptor', 'notificationModalService', 'keepScrollPos', 'marketPlaceService', 'searchCatalogueController', 'MarketPlaceController', 'MarketPlaceDetailsController', 'categorySwiper', 'isLoaded', 'noSpecialCharacters'],
    function (angularAMD) {

        var app = angular.module('tccMarketPlaceApp',
            ['ngRoute',
             'ui.bootstrap',
             'ui.bootstrap.tpls', 'ngTouch']);

        app.controller('appController', ['$location', '$http', function ($location, $http) {

            //var userData = JSON.parse(sessionStorageService.get('auth'));
            //this.fullName = userData.fullName;
            //this.lastLogin = userData.lastLogin;
            //this.menuData = new appMenu().get(userData.roles);

            //this.logout = function () {
            //    $http.put(AppConfig[AppConfig.Environment].apiBaseUri + "Auth/Logout")
            //       .success(function (data) {
            //           window.location.href = 'Login/';
            //       });
            //};

            //this.viewProfile = function () {
            //    $location.path('/Profile/View');
            //};

        }]);

        app.config(['$routeProvider', '$locationProvider',
            function ($routeProvider, $locationProvider) {
                $routeProvider
                    .when('/', angularAMD.route({
                        templateUrl: 'ngViews/MarketPlace.html',
                        controller: 'MarketPlaceController',
                        controllerAs: 'ctrl'

                    }))
                    .when('/UserAuth/:userData?/:jwtToken?', angularAMD.route({
                        templateUrl: 'ngViews/MarketPlace.html',
                        controller: 'MarketPlaceController',
                        controllerAs: 'ctrl'

                    }))
                    .when('/Catalogue/:type/:categoryName/:searchKey?', angularAMD.route({
                        templateUrl: 'ngViews/MarketPlace.html',
                        controller: 'MarketPlaceController',
                        controllerAs: 'ctrl'

                    }))
                    .when('/MarketPlaceDetails/:id/:type?/:categoryName?/:searchKey?', angularAMD.route({
                        templateUrl: 'ngViews/MarketPlaceDetails.html',
                        controller: 'MarketPlaceDetailsController',
                        controllerAs: 'ctrl'

                    })) 
                    .otherwise({
                        redirectTo: '/'
                    });

                //$locationProvider.html5Mode({
                //    enabled: true,
                //    requireBase: false
                //});

                //$locationProvider.html5Mode(true);
            }]);

        app.run(['$location', '$rootScope', '$injector', function ($location, $rootScope, $injector) {
            $rootScope.$on("$routeChangeStart", function (event, next, current) {

                //var authService = $injector.get('authService');
                //if (authService.isAuthorized()) {
                //    //remove notification if any
                //    $('.notify').remove();
                //}
            });
            $rootScope.$on("$routeChangeSuccess", function (event, next, current) {

                //remove notification if any
                //$('.notify').remove();
                //AppMenu.select(next.menuKey);
            });
        }]);
        app.config(['$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push('MarketPlaceServiceInterceptor');
        }]);

        /*app.config(['$httpProvider', function ($httpProvider) {
            //$httpProvider.defaults.cache = false;
            //$httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
            //$httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        }]);*/

        return angularAMD.bootstrap(app);
    });