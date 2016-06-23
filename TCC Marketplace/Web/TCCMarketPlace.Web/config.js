'use strict';
require.config({
    paths: {
        jquery: 'Scripts/jquery-1.9.1.min',
        angular: 'Scripts/angular',
        angularRoute: 'Scripts/angular-route',
        uiBootstrap: 'Scripts/angular-ui/ui-bootstrap.min',
        uiBootstrapTpl: 'Scripts/angular-ui/ui-bootstrap-tpls.min',
        angularTouch: 'Scripts/angular-touch',
        sanitize: 'Scripts/angular-sanitize',
        angularAMD: 'Scripts/angularAMD.min',
        app: 'ngApp/App',
        // uiBootstrapAnimate:'Scripts/angular-animate.min',

        /*register Services - Start*/
        interceptor: 'ngServices/Common/MarketPlaceServiceInterceptor',
        configService: 'ngServices/Common/ConfigService',
        notificationService: 'ngServices/Common/NotificationService',
        notificationModalService: 'ngServices/Common/NotificationModalService',
        marketPlaceService: 'ngServices/MarketPlaceService',
        /*register Services - End*/

        /*register Directives - Start*/
        keepScrollPos: 'ngDirectives/keepScrollPosDirective',
        categorySwiper: 'ngDirectives/swiperDirective',
        isLoaded: 'ngDirectives/isLoadedDirective',
        noSpecialCharacters: 'ngDirectives/noSpecialCharactersDirective',
        /*register Directives - End*/

        searchCatalogueController: 'ngControllers/SearchController',
        MarketPlaceController: 'ngControllers/MarketPlaceController',
        MarketPlaceDetailsController: 'ngControllers/MarketPlaceDetailsController',
    },
    shim: {
        'angular': { deps: ['jquery'], exports: 'angular' },
        'angularRoute': { deps: ['angular'] },
        'uiBootstrap': { deps: ['angular'] },
        'uiBootstrapTpl': { deps: ['angular'] },
        'angularTouch': { deps: ['angular'] },
        'sanitize': { deps: ['angular'] },
        'angularAMD': { deps: ['angular'] }
    },
    deps: ['app']
});