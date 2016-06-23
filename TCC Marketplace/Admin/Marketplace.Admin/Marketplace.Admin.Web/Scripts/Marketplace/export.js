/// <reference path="../site.js" />

$(document).ready(function () {

    $('#Frequncies').change(function () {
        $('#btnSubmit').prop('disabled', false);
    });

    $('#btnSubmit').click(function () {
        showAjaxLoader();
        var frequencyId = $('#Frequncies').val();
        $.ajax({
            url: "/Export/UpdateFrequency",
            data: { frequencyId: frequencyId },
            dataType: "json",
            type: "POST"            
        })
        .done(function (data) {
            if (data.success == true) {
                $("#divMessage").removeClass("error").addClass("success");
            }
            else {
                $("#divMessage").removeClass("success").addClass("error");
            }
            $('#divMessage').text(data.message);
            $('#btnSubmit').prop('disabled', true);
        })
        .fail(function () {
            $("#divMessage").removeClass("success").addClass("error");
            $('#divMessage').text("Failed to update extract frequency");
        })
        .always(function () {
            hideAjaxLoader();
            $('#divMessage').fadeIn(200).delay(3000).fadeOut(200);
        });
    });

    $('#btnRunNow').click(function () {
        showAjaxLoader();
        $.ajax({
            url: "/Export/RunExtract",
            dataType: "json",
            type: "POST"
        })
        .done(function (data) {
            if (data.success == true) {
                $("#divMessage").removeClass("error").addClass("success");
            }
            else {
                $("#divMessage").removeClass("success").addClass("error");
            }
            $('#divMessage').text(data.message);
        })
        .fail(function () {
            $("#divMessage").removeClass("success").addClass("error");
            $('#divMessage').text("Failed to execute extract job");
        })
        .always(function () {
            hideAjaxLoader();
            $('#divMessage').fadeIn(200).delay(3000).fadeOut(200);
        });
    });
});