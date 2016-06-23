
var Login = function () {

    var self = this;

    self.registerEvents = function () {

        //register login click
        $('#frmLogin').submit(function () {
            //var loginPromise = loginService.login($('#txtUsername').val(), $('#txtPassword').val());

            $('#spinner').show();
            $('[type=submit]').attr('disabled', 'disabled');
            window.location.href = '/';
            //loginPromise.then(function (data) {
               // localStorage.setItem('auth', JSON.stringify(data));
                //window.location.href = '../';
            //}, function (ex) {

            //    $('#spinner').hide();
            //    $('[type=submit]').attr('disabled', false);
            //    $('#loginErr').text(ex).show();
            //});

            return false;
        });
               

       
    };

    var loginService = {
        login: function (un, pwd) {
            var deff = jQuery.Deferred();

            $.post(AppConfig[AppConfig.Environment].apiBaseUri + "Auth/Login", { UserName: un, Password: pwd })
                .then(function (data) {
                    if (data.hasError) {
                        deff.reject(data.errorMessage);
                    }
                    deff.resolve(data.data);
                }, function (ex) {
                    deff.reject(ex);
                });
            return deff.promise();
        }
    };
       


    self.init = function () {
        //localStorage.removeItem('auth');
        self.registerEvents();
    };
};



$(document).ready(function () {
    var login = new Login();
    login.init();

});


