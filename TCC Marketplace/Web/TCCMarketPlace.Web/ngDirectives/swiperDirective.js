"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('categorySwiper', ['$timeout', function ($timeout) {
        return {
            link: function (scope, element, attr) {
                scope.$on('content-changed', function () {
                    new Swiper('.swiper-container', {
                        pagination: '.swiper-pagination',
                        paginationClickable: true,
                        initialSlide: 0,
                        slidesPerView: 3,
                        spaceBetween: 75,
                        breakpoints: {
                            1024: {
                                slidesPerView: 3,
                                spaceBetween: 60
                            },
                            768: {
                                slidesPerView: 3,
                                spaceBetween: 30
                            },
                            640: {
                                slidesPerView: 3,
                                spaceBetween: 10
                            },
                            375: {
                                slidesPerView: 3,
                                spaceBetween: 20
                            },
                            360: {
                                slidesPerView: 3,
                                spaceBetween: 10
                            },
                            320: {
                                slidesPerView: 3,
                                spaceBetween: 25
                            }
                        }
                    });

                });

            }
        };
    }])

});


