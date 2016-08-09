/// <reference path="../site.js" />

$(document).ready(function () {
    hideAjaxLoader();
    var maxDate = new Date($('#EndDate').val());
    if (maxDate.isValid()) {
        maxDate.setDate(maxDate.getDate() - 1);
        $('#StartDate').datepicker('setEndDate', maxDate);
    }

    var minDate = new Date($('#StartDate').val());
    if (minDate.isValid()) {
        minDate.setDate(minDate.getDate() + 1);
        $('#EndDate').datepicker('setStartDate', minDate);
    }

    $('#sliderFile').change(function () {
        var input = this;

        if (input.files && input.files[0]) {
            var filerdr = new FileReader();
            filerdr.onload = function (e) {
                var img = new Image();
                img = $('#prvSlider');
                img.attr('src', e.target.result);
                img.on("load", function () {
                    var newSize = scaleSize(620, 250, img[0].naturalWidth, img[0].naturalHeight);
                    img.width(newSize[0]).height(newSize[1]);
                });
            }
            filerdr.readAsDataURL(input.files[0]);
            $('#SliderImage').val(input.value);
        }
    });

    $('#deleteSlider').click(function () {
        var input = $('#sliderFile');
        input.empty();
        img = $('#prvSlider');
        img.attr('src', null);
        $('#SliderImage').val("");

    });

    $('#iconFile').change(function () {
        var input = this;
        if (input.files && input.files[0]) {
            var filerdr = new FileReader();
            filerdr.onload = function (e) {
                var img = new Image();
                img = $('#prvIcon');
                img.attr('src', e.target.result);
                img.on("load", function () {
                    var newSize = scaleSize(130, 130, img[0].naturalWidth, img[0].naturalHeight);
                    img.width(newSize[0]).height(newSize[1]);
                });
            }
            filerdr.readAsDataURL(input.files[0]);
            $('#IconImage').val(input.value);
        }
    });

    $('#add_product').click(function () {
        $('#btnAddProduct').val('Add');
        $('#ProductCategories').prop('selectedIndex', 0);
        $("#ProductCategories").removeAttr("disabled");
        $('#dvProducts').empty();
        $('#product-pops').show();
        return false;
    });

    $('#add_location').click(function () {
        $('#CountryList').prop('selectedIndex', 0);
        $("#CountryList").removeAttr("disabled");
        $("#StateList").removeAttr("disabled");
        $('#CountryList').change();
        $('#btnSaveLoc').val('Add');
        $('#dvZFC').empty();
        $('#chkAllStates').attr('checked', false);
        $('#chkAllCountries').attr('checked', false);
        $("#chkAllStates").attr("disabled", true);
        $("#chkAllCountries").removeAttr("disabled");
        $('#click-pops').show();
        return false;
    });

    $("#StartDate").datepicker().on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        minDate.setDate(minDate.getDate() + 1);
        $('#EndDate').datepicker('setStartDate', minDate);
    });

    $("#EndDate").datepicker().on('changeDate', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        maxDate.setDate(maxDate.getDate() - 1);
        $('#StartDate').datepicker('setEndDate', maxDate);
    });


    $("#btnSave").on("click", function () {
        return validateForm();
    });

    var validateForm = function () {
        var form = $("#serviceForm");
        if ($("input[name=MakeLive]:checked").val() == "True" && $("#InAppPurchaseId").val().length == 0) {
            $("span[data-valmsg-for=InAppPurchaseId]").text("In-app purchase ID is required when Make live value is 'Yes'.");
            form.valid();
            return false;
        }
        else {
            $("span[data-valmsg-for=InAppPurchaseId]").empty();
            if (form.valid()) {
                showAjaxLoader();
                return true;
            }
        }

    }

    $("#btnToogleStatus").click(function () {
        showAjaxLoader();
        var currentStatus = $('#IsActive').val();
        var serviceId = $('#Id').val();
        var message = "Are you sure you want to " + (currentStatus == "True" ? "deactivate" : "activate") + " this service ?";
        var option = confirm(message);
        var flag = currentStatus == "True" ? "False" : "True";
        if (option == true) {
            $.ajax({
                url: "/Services/UpdateServiceStatus",//"@Url.Action("UpdateServiceStatus", "Services")",
                data: { serviceId: serviceId, flag: flag },
                dataType: "json",
                type: "POST"
            })
            .done(function (data) {
                if (data == true) {
                    $('#IsActive').val("True");
                    $("#btnToogleStatus").val("Deactivate Service");
                    $("#imgActive").attr("src", "/Content/images/tick.png");
                    $("#imgActive").next('span').text("Active");
                } else {
                    $('#IsActive').val("False");
                    $("#btnToogleStatus").val("Activate Service");
                    $("#imgActive").attr("src", "/Content/images/cross.png");
                    $("#imgActive").next('span').text("Inactive");
                }
            })
            .fail(function () {
                //alert("Unable to populate States.");
                hideAjaxLoader();
            })
            .always(function () {
                hideAjaxLoader();
            });
        } else {
            hideAjaxLoader();
            return false;
        }
    });

    $("#ServiceProviderId").on("change", function () {
        var srvPvdrId = $("#ServiceProviderId").val();
        $.ajax({
            type: "GET",
            url: "/Services/GetSignUpUrl",
            data: { id: srvPvdrId },
            datatype: 'json'
        })
        .done(function (signUpUrl) {
            $("#URL").val(signUpUrl);
        });

    });

});