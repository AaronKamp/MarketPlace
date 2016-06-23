"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('isLoaded', function () {
        return {
            scope: false, //don't need a new scope
            restrict: 'A', //Attribute type
            link: function (scope, elements) {

                if (scope.$last) {
                    scope.$emit('content-changed');
                    console.log('page Is Ready!');
                }
            }
        };
    })

});


