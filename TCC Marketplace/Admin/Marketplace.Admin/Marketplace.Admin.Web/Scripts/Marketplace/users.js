/// <reference path="../site.js" />
$(function () {

    var currentPage = 1;

    ReloadGrid(currentPage);
    
    $(".pagination").on("click", "ul li a", function (e) {
        e.preventDefault();
        var page = getParameterByName('page', $(this).attr("href"));
        ReloadGrid(page);
    });

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function ReloadGrid(page) {
        showAjaxLoader();
        $.ajax({
            url: "/User/GetUsers",
            type: "GET",
            data: { 'page': page },
            datatype: 'json'
        })
        .done(function (data) {
            BindUserGrid(data);
        })
        .fail(function (data) {
            $("#divMessage").removeClass("success").addClass("error");
            $("#divMessage").html("Data could not be loaded.").fadeIn(200).delay(3000).fadeOut(200);
        })
        .always(function () {
            hideAjaxLoader();
        });
    }

    function BindUserGrid(viewModel) {
        var button = $("#userGrid").find("tr:last");
        $("#userGrid").find("tr:gt(0)").remove();
        $(".pagination").html("");
        var users = viewModel.Users;
        for (var i in users) {
            var newRow = "<tr>"
                           + "<td align='center'>" + users[i].RowNo + "</td>"
                           + "<td align='left'>" + users[i].UserName + "</td>"
                           + "<td align='left'>" + users[i].Email + "</td>"
                           + "<td align='center'>"
                           + "<a class='btnEditUser' href='" + users[i].Id + "'>"
                           + "<img src='/Content/Images/edit.png' /></a> </td></tr>";
            $("#userGrid").first('tr').append(newRow);
        }

        $("#userGrid").first('tr').append(button);
        if (viewModel.NoOfPages > 1) {
            var pagerHTML = "<ul>";
            for (i = 1; i <= viewModel.NoOfPages; ++i) {
                pagerHTML = pagerHTML + "<li> <a href='#/?page=" + i + "'>" + i + "</a></li>";
            }
            var nextPageNo = viewModel.CurrentPage + 1;
            if (viewModel.CurrentPage == viewModel.NoOfPages) {
                nextPageNo = 1;
            }
            pagerHTML = pagerHTML + "<li><a href='#/?page=" + nextPageNo + "' class='last'>&nbsp;</a></li>";
            $(".pagination").append(pagerHTML);
            currentPage = viewModel.CurrentPage;
        }
    }

    $("#close").click(function () {
        $("#popup-box").hide();
    });

    $("#userGrid").on("click", "tr td a#btnAddUser", function (e) {
        e.preventDefault();
        $("#popup-box h2").html("Add New User");
        $(".validation-summary-errors ul").html("");
        $(".validation-summary-errors").toggleClass("validation-summary-errors validation-summary-valid");
        $(".field-validation-error span").hide();
        $("#popup-box").show();
        $(".pop-content").show();
        $("#user-form input").val("");
        $(".pwd-field-btn").hide();
        $(".pwd-field").show();
        $("#user-form input[name='UserName']").removeAttr('readonly');
    });

    $("#userGrid").on("click", "tr td a.btnEditUser", function (e) {
        e.preventDefault();
        userId = $(this).attr("href");
        $("#user-form input").val("");
        showAjaxLoader();
        $.ajax({
            url: "/User/GetUser",
            type: "GET",
            data: { "id": userId },
            datatype: 'json'
        })
        .done(function (data) {
            $("#user-form input[name='UserName']").attr('readonly', 'true');
            $("#user-form input[name='UserName']").val(data.UserName);
            $("#user-form input[name='Email']").val(data.Email);
            $("#user-form input[name='Id']").val(data.Id);
        })
        .fail(function () {
            $("#divMessage").removeClass("success").addClass("error");
            $("#divMessage").html("Data could not be loaded.").fadeIn(200).delay(3000).fadeOut(200);
        }).always(function () {
            $("#popup-box h2").html("Edit User");
            $(".field-validation-error span").hide();
            $(".validation-summary-errors ul").html("");
            $(".validation-summary-errors").toggleClass("validation-summary-errors validation-summary-valid");
            $("#popup-box").show();
            $(".pop-content").show();
            $(".pwd-field-btn").show();
            $(".pwd-field").hide();
            hideAjaxLoader();
        });
    });

    $("#btnPwdChange").on("click", function (e) {
        e.preventDefault();
        $(".pwd-field-btn").hide();
        $(".pwd-field").show();
    });

    $("#btnSave").click(function (e) {
        e.preventDefault();
        var form = $("#user-form");
        if (form.valid()) {
            var url = "/User/AddNewUser";
            var userData = form.serialize();
            $.post(url, userData)
            .done(function (data) {
                if (data.Status.toLowerCase() == "ok") {
                    $("#popup-box").hide();
                    ReloadGrid(currentPage);
                    $("#divMessage").removeClass("error").addClass("success");
                    $("#divMessage").html(data.Message).fadeIn(200).delay(3000).fadeOut(200);                   
                }
                else {
                    $(".validation-summary-valid").toggleClass("validation-summary-valid validation-summary-errors");
                    var list = data.Message;
                    var errorList = $(".validation-summary-errors ul").html("");
                    $.each(list, function (i) {
                        $("<li/>")
                            .html(list[i])
                            .appendTo(errorList);
                    });
                }
            })
            .fail(function (data) {
                $("#popup-box").hide();
                $("#divMessage").removeClass("success").addClass("error");
                $("#divMessage").html(data.Message).fadeIn(200).delay(3000).fadeOut(200);
            });
        }
    });

    
});