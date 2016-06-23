
$(function () {

    if ($("#is-password-protected").is(":checked")) {
        $("#ssh-password").prop("disabled", false);
    }
    else {
        $("#ssh-password").prop("disabled", true);
    }

    $("input[type = radio][id = sshKeyOption]").on("change", function () {
        $("#btnSshPwdChange").hide();
        $("#sshPwdField").show();
        if (this.value == 'ReadFromFile') {
            $("#ssh-key-text").attr("disabled","true");
            $("#ssh-key-text").hide();
            $("#sshKeyFile").show();
        }
        else if (this.value == 'Paste') {
            $("#ssh-key-text").removeAttr("disabled");
            $("#sshKeyFile").hide();
            $("#ssh-key-text").show();
        }
    });

    $("#is-password-protected").on("change", function () {

        if ($("#is-password-protected").is(":checked")) {
            $("#ssh-password").prop("disabled",false);
        }
        else {
            $("#ssh-password").prop("disabled", true);
        }

    });

    $("#btnSshPwdChange").on("click", function () {
        $("#btnSshPwdChange").hide();
        $("#sshPwdField").show();
    });

    $("#btnEmailPwdChange").on("click", function () {
        $("#btnEmailPwdChange").hide();
        $("#emailPwdField").show();
    });
    
    $("#saveSettings").on("click", function () {
        
        return validateForm();
    });


    $("#emailPwdField input").change(function () {
        $("#emailPwdField span").html("");
    });
    $("#sshPwdField input").change(function () {
        $("#sshPwdField span").html("");
    });


    var validateForm = function(){
        var form = $("#settingsForm");
        var isValid = true;
            if ($("#emailPwdField").is(":visible")) {
                if ($("#emailPwdField input").val() == "") {
                    $("#emailPwdField span").html("Enter from email password");
                    isValid = false;
                }
            }
            if ($("#sshPwdField").is(":visible") && $("#is-password-protected").is(":checked")) {
                if ($("#sshPwdField input").val() == "") {
                    $("#sshPwdField span").html("Enter from email password");
                    isValid = false;
                }
            }
            form.valid();
            if (isValid)
                return true;
            else return false;
    }



});