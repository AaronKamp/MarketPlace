'use strict';
define(['angularAMD', 'app'], function (angularAMD, app) {

    angularAMD.factory('MarketPlaceServiceInterceptor', ['$q', '$injector', '$location', function ($q, $injector, $location) {
        return {
            request: function (config) {
                config.headers = config.headers || {};                
                //$('#ajaxSpinner').height($(document).height());
                //$('#ajaxSpinner').show();

                config.headers["User-Data"] = localStorage.getItem("user_data");
                config.headers["Authorization"] = localStorage.getItem("jwt_token");

                return config || $q.when(config);
            },
            requestError: function (rejection) {

                 //$('#ajaxSpinner').hide();
                return $q.reject(rejection);
            },
            response: function (response) {
               // $('#ajaxSpinner').hide();
                if (response.data.hasError) {
                    var notificationService = $injector.get('notificationModalService');
                    notificationService.open({ header: "Error", message: response.data.errorMessage });
                    return $q.reject(response);
                }
                

                var notificationService = $injector.get('notificationService');
                notificationService.close();

                return response || $q.when(response);
            },

            responseError: function (rejection) {
                //$('#ajaxSpinner').hide();
                var notificationService = $injector.get('notificationModalService');

                if (rejection.status === 401) {                    
                    return $q.reject(rejection);
                }
                else if (rejection.status === -1) {
                    notificationService.open({ header: "Error", message: "Something is not right! Please try after some time." });
                } else {
                    notificationService.open({ header: "Error", message: rejection.data.message });
                }
                return $q.reject(rejection);


            }
        };
    }]);
});