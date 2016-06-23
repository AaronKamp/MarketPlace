"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('noSpecialCharacters', function () {
        return {
            require:'ngModel',
            restrict: 'A', //Attribute type
            link: function (scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (inputValue) {
                   
                    if (inputValue == undefined) {
                        return '';
                    }
                    var cleanInputValue = inputValue.replace(/[^0-9a-zA-Z\s\-]/gi, '');
                    if (cleanInputValue != inputValue) {
                        modelCtrl.$setViewValue(cleanInputValue);
                        modelCtrl.$render();
                    }
                    return cleanInputValue;
                });
            }
        };
    })

});