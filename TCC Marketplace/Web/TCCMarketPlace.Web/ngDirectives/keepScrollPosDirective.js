"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive("keepScrollPos", ['$route','$window','$timeout','$location', '$anchorScroll',function ($route, $window, $timeout, $location, $anchorScroll) {

        // cache scroll position of each route's templateUrl
        var scrollPosCache = {};

        // compile function
        return function (scope, element, attrs) {

            scope.$on('$routeChangeStart', function () {
                // store scroll position for the current view
                if ($route.current) {
                    scrollPosCache[$route.current.loadedTemplateUrl] = [$window.pageXOffset, $window.pageYOffset];
                }
            });

            scope.$on('$routeChangeSuccess', function () {
                $timeout(function () {
                    // if hash is specified explicitly, it trumps previously stored scroll position
                    if ($location.hash()) {
                        $anchorScroll();

                        // else get previous scroll position; if none, scroll to the top of the page
                    } else {
                        var prevScrollPos = scrollPosCache[$route.current.loadedTemplateUrl] || [0, 0];
                        $window.scrollTo(prevScrollPos[0], prevScrollPos[1]);

                    }
                }, 300);
            });
        }
    }]);

});

