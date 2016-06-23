/// <reference path="../site.js" />

$('#CountryList').change(function () {
    
    var ddlState = $('#StateList');
    ddlState.empty();
    $('#dvZFC').empty();
    $('#chkAllStates').attr("checked", false);
    $('#chkAllCountries').attr("checked",false);
    $("#chkAllStates").attr("disabled",true);
    $("#chkAllCountries").removeAttr("disabled");
    ddlState.append('<option value= 0> State</option>');
    var countryId = this.value;
    if (countryId > 0) {
        $("#chkAllStates").removeAttr("disabled");
        showAjaxLoader();
        $.ajax({

            url: "/Services/GetStates",//"@Url.Action("GetStates", "Services")",
            data: { countryId: countryId },
            dataType: "json",
            type: "GET"
        })
        .done(function (data) {
            if (data.length == 0) {
                $('#dvZFC').html('<span class="text-danger">State list is empty! </span>');
                $("#chkAllStates").attr("disabled", true);
                $('#btnSaveLoc').attr('disabled', true);
            }
            else {
                $("#chkAllStates").removeAttr('disabled');
                $('#btnSaveLoc').removeAttr('disabled');
                $.each(data, function (i) {
                    var optionhtml = '<option value="' +
                data[i].Value + '">' + data[i].Text + '</option>';
                    ddlState.append(optionhtml);
                });
            }
        })
        .fail(function () {
            alert("Unable to populate States.");
        })
        .always(function () {
             hideAjaxLoader();    
        });

    }
});

$('#StateList').change(function () {

    var SFC_div = $('#dvZFC');
    SFC_div.empty();
    $('#chkAllStates').checked = false;
    $('#chkAllCountries').checked = false;
    $("#chkAllStates").removeAttr("disabled");
    $("#chkAllCountries").removeAttr("disabled");

    var stateId = this.value;
    if (stateId > 0) {
        populateSCFs(stateId);
    }
});

$('#chkAllCountries').change(function () {
    $('#CountryList').prop('selectedIndex', 0);
    $('#StateList').prop('selectedIndex', 0);
    $('#dvZFC').empty();
    $('#chkAllStates').attr('checked', false);

    if (this.checked) {
        $('#CountryList').attr("disabled", "disabled");
        $('#StateList').attr("disabled", "disabled");
        $('#chkAllStates').attr("disabled", "disabled");
    }
    else {
        $("#CountryList").removeAttr("disabled");
        $("#StateList").removeAttr("disabled");
        $("#chkAllStates").removeAttr("disabled");
    }
});

$('#chkAllStates').change(function () {
    $('#StateList').prop('selectedIndex', 0);
    $('#dvZFC').empty();
    $('#chkAllCountries').checked = false;

    if (this.checked) {
        $('#CountryList').attr("disabled", "disabled");
        $('#StateList').attr("disabled", "disabled");
        $('#chkAllCountries').attr("disabled", "disabled");
    }
    else {
        $("#CountryList").removeAttr("disabled");
        $("#StateList").removeAttr("disabled");
        $("#chkAllCountries").removeAttr("disabled");
    }
});

$('#btnSaveLoc').click(function () {

    showAjaxLoader();

    if ($('#chkAllCountries').is(':checked') || $('#chkAllStates').is(':checked')) {

        var countryId = 0;

        if ($('#chkAllStates').is(':checked') == true) {
            countryId = $("#CountryList").val();
        }

        $.ajax({
            url: "/Services/LoadSCFs",//"@Url.Action("LoadSCFs", "Services")",
            data: { countryId: countryId },
            dataType: "json",
            type: "GET"
        })
        .done(function (data) {
            $.each(data, function (i, item) {
                var loc = data[i];
                BindLocation(loc);
            });

            $('#click-pops').hide();
        })
        .fail(function () {
            alert("Unable to populate SCFs.");
        })
        .always(function () {
            hideAjaxLoader();
        });
    }
    else {

        if ($('#CountryList').val() == 0) {
            alert('Select a country');
            $('#CountryList').focus();
            hideAjaxLoader();
            return false;
        }

        if ($('#StateList').val() == 0) {
            alert('Select a state');
            $('#StateList').focus();
            hideAjaxLoader();
            return false;
        }

        if ($('#dvZFC input:checked').length == 0) {
            alert('Select a SCF Code');
            hideAjaxLoader();
            return false;
        }

        var location = {};
        location.CountryId = $('#CountryList').val();
        location.CountryName = $('#CountryList :selected').text();
        location.StateId = $('#StateList').val();
        location.StateName = $('#StateList :selected').text();
        location.SCFs = [];
        location.SCFNames = ''
        $('#dvZFC input:checked').each(function () {
            location.SCFs.push($(this).val());
        });
        $('#dvZFC input:checked').each(function () {
            location.SCFNames += ($(this).attr('name')) + ', ';
        });

        BindLocation(location);
        $('#click-pops').hide();
        hideAjaxLoader();
    }
});

function BindLocation(loc) {

    var tblLoc = $('#tblLocation');

    if ($("#trLocation" + loc.StateId).length > 0) {
        $("#trLocation" + loc.StateId).remove();
    }

    var locationJString = fixedEncodeURIComponent(JSON.stringify(loc));

    var newRow = "<tr id='trLocation" + loc.StateId + "'>  <input type='hidden' value='" + locationJString + "' name='Location' />"
                        + '<td>' + loc.CountryName + '</td>'
                        + '<td>' + loc.StateName + '</td>'
                        + '<td><span class="trim-column" style="width:250px">' + loc.SCFNames + '</span></td>'
                        + '<td align="center"><a href="#" name="editLocation"> <img src="/Content/images/edit.png"> </a></td>'
                        + '<td align="center"><a href="#" name="deleteLocation"><img src="/Content/images/delete.png"> </a></td>'
                   + '</tr>'
    tblLoc.append(newRow);
}


$(document).on('click', "#tblLocation tr td a[name='editLocation']", function (e) {

    e.preventDefault();

    $('#btnSaveLoc').val('Update');
    
    ResetPopUpControls();

    var jsonString = $(this).closest('td').closest('tr').find('input[type="hidden"][name="Location"]').val();

    var location = JSON.parse(decodeURIComponent(jsonString));
    BindLocationPopUp(location);

});

$(document).on('click', "#tblLocation tr td a[name='deleteLocation']", function (e) {
    e.preventDefault();
    $(this).closest('td').closest('tr').remove();
});

function ResetPopUpControls() {
    $('#dvZFC').empty();
    $('#chkAllStates').attr('checked', false);
    $('#chkAllCountries').attr('checked', false);
    $("#chkAllStates").attr("disabled", true);
    $("#chkAllCountries").attr("disabled", true);
    $('#CountryList').attr("disabled", "disabled");
    $('#StateList').attr("disabled", "disabled");
}

function BindLocationPopUp(location) {
    $('#click-pops').show();

    $('#CountryList').val(location.CountryId);

    $('#StateList').empty();
    optionhtml = '<option value="' +
               location.StateId + '">' + location.StateName + '</option>';

    $('#StateList').append(optionhtml);
    $('#StateList').val(location.StateId);
    populateSCFs(location.StateId);
    disableCheckBoxes();
}

var disableCheckBoxes = function() {

    $("#chkAllStates").attr("disabled", true);
    $("#chkAllCountries").attr("disabled", true);
}

var populateSCFs = function (stateID) {
     var SFC_div = $('#dvZFC');
        SFC_div.empty();
    showAjaxLoader();
    $.ajax({
        url: "/Services/GetSCFs",//"@Url.Action("GetSCFs", "Services")",
        data: { stateId: stateID },
        dataType: "json",
        type: "GET",
    }).done(function (data) {
       
        if (data.length == 0) {
            $('#dvZFC').html('<span class="text-danger">SCF list is empty! </span>');
            $('#btnSaveLoc').attr('disabled', true);
        }
        else {
            
            $.each(data, function (i) {

                var checkboxhtml = '<label><input type="checkbox" value="' +
                                    data[i].Value + '" name="' + data[i].Text + '" id="chkSCF' + data[i].Value + '"/>&nbsp;&nbsp;' + data[i].Text + '</label>';
                SFC_div.append(checkboxhtml);
            });

            var locString = $("#trLocation" + stateID).find('input[type="hidden"][name="Location"]');
            if (locString.length > 0) {
                var jsonString = locString.val();
                var location = JSON.parse(decodeURIComponent(jsonString));
                $.each(location.SCFs, function (index, value) {
                    $('#chkSCF' + value).attr('checked', true);
                });
            }
            else {
                $('#dvZFC :checkbox:enabled').prop('checked', true);
            }
            $('#btnSaveLoc').removeAttr('disabled');
        }
    }).fail(function () {
        alert("Unable to populate SCFs.");

    }).always(function () {
        hideAjaxLoader();
    });
    
}