'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('configService', function () {

        this.getApiBaseUrl = function () {
            
            var apiBaseUri = decodeURIComponent(localStorage.getItem("base_uri"));

            if (apiBaseUri == null || apiBaseUri === "null" || apiBaseUri === "undefined") {

                var locationHash = window.location.hash;
                apiBaseUri = locationHash.substring(locationHash.indexOf("http"));
                localStorage.setItem("base_uri", apiBaseUri);
            }

            if (apiBaseUri.endsWith("/") === false) {
                apiBaseUri = apiBaseUri + "/";
            }

            return apiBaseUri;
        };



    });

});