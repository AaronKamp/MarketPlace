'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('configService', function () {

        this.getApiBaseUrl = function () {
            return AppConfig[AppConfig.Environment].apiBaseUri;
        };       

        this.getAppUrl = function () {
            return AppConfig[AppConfig.Environment].appHomePage;
        };

    });

});