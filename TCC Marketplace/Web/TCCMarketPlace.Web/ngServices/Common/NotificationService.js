"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('notificationService', function () {

        this.notify = function (message, config) {

            var container = config.container || 'body',
                timeout = config.timeout || 2000,
                type = config.type || 'success',
                top = config.container == undefined ? '45px' : $(config.container).offset().top + 'px',
                left = config.container == undefined ? '0px' : $(config.container).offset().left - (parseInt($(config.container).css('width')) / 2) + 'px',
                autoClose = config.autoClose == undefined ? true : config.autoClose,
                deff = jQuery.Deferred();

            $('.notify').remove();
            $(container).prepend('<div type="' + type + '"  class="notify alert alert-' + type + '" style="position:fixed;margin:0px 35%;top:' + top + ';left:' + left + '">' + message + '</div>');

            if (autoClose) {
                setTimeout(function () {
                    $('.notify').hide(function () {
                        $('.notify').remove();
                        deff.resolve(true);
                    });
                }, timeout);
            }
            
            else {
                deff.resolve(true);
            }

            return deff.promise();
        };

        this.close = function () { $('.notify').remove(); };

        this.AppendString = function (curentString, strToBeAppended) {
            if (curentString != "") {
                return curentString + "<br/>" + strToBeAppended;
            } else {
                return strToBeAppended;
            }
        };

    });

});