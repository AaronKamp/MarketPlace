/// <reference path="../site.js" />

$(function () {

    ReloadGrid(1);

    $('#CountryList').change(function () {
        var ddlState = $('#StateList');
        ddlState.empty();
        ddlState.append('<option value= 0> State</option>');
        var countryId = this.value;
        if (countryId > 0) {
            $.ajax({
                url: "/Services/GetStates",
                data: { countryId: countryId },
                dataType: "json",
                type: "GET"
            })
            .done(function (data) {
                $.each(data, function (i) {
                    var optionhtml = '<option value="' +
                data[i].Value + '">' + data[i].Text + '</option>';
                    ddlState.append(optionhtml);
                });
            })     
            .fail(function () {
                alert("Error!");
            });
        }
    });

    $('#btnGo').click(function () {
        ReloadGrid(1)
    });

    $('#btnSearch').click(function () {
        ReloadGrid(1);
    });

    //$('#service_grid thead th a, tfoot a').click(function () {
    //    console.log(this); e.preventDefault();
    //});
    $(".pagination").on("click", "ul li a", function (e) {
        e.preventDefault();
        var page = getParameterByName('page', $(this).attr('href'));
        ReloadGrid(page);
        $(".service-table.clear").scrollLeft(0);
    });

    function ReloadGrid(page) {
        var url = "/Services/GetServices";

        var country = '';
        if ($('#CountryList option:selected').val() != 0) {
            country = $('#CountryList option:selected').text();
        }

        var state = '';
        if ($('#StateList option:selected').val() != 0) {
            state = $('#StateList option:selected').text();
        }

        showAjaxLoader();

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                'country': country,
                'state': state,
                'keywords': $('#txtKeyWords').val(),
                'SCFs': $('#txtSCF').val(),
                'zipCodes': $('#txtZipCode').val(),
                'thermostats': $('#txtThermostsat').val(),
                'page': page
            },
            datatype: 'json'
        })
        .done(function (data) {
            BindServiceGrid(data);
        })
        .fail(function () {
            alert('Error!');
        })
        .always(function(){
            hideAjaxLoader();

        });
    }

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function BindServiceGrid(viewModel) {
        $("#servicesGrid").find("tr:gt(0)").remove();
        $('.pagination').html('');
        var services = viewModel.Services;

        for (var i in services) {

            var newRow = "<tr>"
                         + "<td align='center'>" + services[i].RowNo + "</td>"
               + "<td class='row-title'>"
                  + "<img src='" + services[i].IconImage + "' align='left' style='height:45px; width:45px'>"
                  + "<strong>" + services[i].Title + "</strong><br />"
                  + services[i].ShortDescription
               + "</td>"
               + "<td align='center'>" + services[i].Type + "</td>"
               + "<td align='center'>" + Getvalue(services[i].ProductId) + "</td>"
               + "<td class='colspace'>" + trimText(Getvalue(services[i].Countries),12) + "</td>"
               + "<td class='colspace'>" + trimText(Getvalue(services[i].States),45) + "</td>"
               + "<td align='center'>" + formatJsonDate(services[i].StartDate) + "</td>"
               + "<td align='center'>" + formatJsonDate(services[i].EndDate) + "</td>";
            if (services[i].IsActive == true) {
                newRow = newRow + "<td align='center'><img src='../Content/images/tick.png'>" + "</td>"
            }
            else {
                newRow = newRow + "<td align='center'><img src='../Content/images/cross.png'>" + "</td>"
            }
            newRow = newRow + "<td align='center'><a href='/Services/Edit/" + services[i].ServiceId + "'><img src='../Content/images/edit.png'>" + "</td>" + "</tr>";

            $("#servicesGrid").first('tr').append(newRow);
        }

        if (viewModel.NoOfPages > 1) {

            var pagerHtml = "<ul>";
            for (i = 1; i <= viewModel.NoOfPages; i++) {
                pagerHtml = pagerHtml + "<li><a href='#/?page=" + i + "'>" + i + "</a></li>";
            }

            var nextPageNo = viewModel.CurrentPage + 1;
            if (viewModel.CurrentPage == viewModel.NoOfPages) {
                nextPageNo = 1;
            }

            pagerHtml = pagerHtml + "<li><a href='#/?page=" + nextPageNo + "' class='last'>&nbsp;</a></li>";
            pagerHtml = pagerHtml + "</ul>";

            $('.pagination').append(pagerHtml);
        }
    }

    function formatJsonDate(jsonDate) {
        var dateString = jsonDate.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var date = month + "/" + day + "/" + year;
        return date;
    };

    function Getvalue(string) {
        if (string == null) {
            return "";
        }
        else {
            return string;
        }
    }

});