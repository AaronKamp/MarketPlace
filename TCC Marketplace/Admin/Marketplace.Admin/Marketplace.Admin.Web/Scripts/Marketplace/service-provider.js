$(function () {

    if ($("#GenerateBearerTokenYes").is(':checked')) {
        //  $("#TokenUrl").attr("data-val-required", "Token URL field is required when Generate bearer token is enabled.");
        $("#TokenUrl").attr("required", true);
        $("#AppId").attr("required", true);
        $("#SecretKey").attr("required", true);

    }
    hideAjaxLoader();

    $("#ProviderList").on("change", function () {
        showAjaxLoader();
        $("#new-srvProvider").hide();
        var srvPvdr = {
            name: $('#ProviderList option:selected').text(),
            id: $('#ProviderList').val() > 0 ? $('#ProviderList').val() : 0
        };

        $.ajax({
            type: 'GET',
            url: '/ServiceProvider/Details',
            data: { id: srvPvdr.id },
            dataType: 'html'
        })
        .done(function (view) {
            $('#detials-container').empty();
            $("#Name").val(srvPvdr.name);
            $('#details-container').html(view);
        })
        .always(function () {
            if (srvPvdr.id == 0) {
                $("#new-srvProvider").show();
            }
            hideAjaxLoader();
        });

    });

    $("#new-srvProvider").on("click", function () {
        $("#txt-srvProviderName").removeClass("hide");
        $("#list-srvProviderName").hide();
    });

    $("#reset-btn").click(function () {
        showAjaxLoader();
        var confirmation = confirm("Unsaved data will be lost. Are you sure to reset the form?");
        if (!confirmation) {
            hideAjaxLoader();
            return false;
        }
    });

    $("#Save").on("click", function () {
        $("#srv-providerform").valid();
    });

    $("#GenerateBearerTokenYes").change(function () {
        if ($("#GenerateBearerTokenYes").is(':checked')) {
            // $("#TokenUrl").attr("data-val-required", "Token URL field is required when Generate bearer token is enabled.");
            $("#TokenUrl").attr("required", true);
            $("#AppId").attr("required", true);
            $("#SecretKey").attr("required", true);
        }
    });

    $("#GenerateBearerTokenNo").change( function () {
        if ($("#GenerateBearerTokenNo").is(':checked')) {
            $("#TokenUrl").removeAttr('required');
            $("#AppId").removeAttr('required');
            $("#SecretKey").removeAttr('required');
        }
    });
});