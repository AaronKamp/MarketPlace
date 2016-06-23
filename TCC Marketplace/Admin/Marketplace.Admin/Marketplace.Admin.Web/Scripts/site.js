$(function () {
    // Hide show scroll to top arrow
    $(window).scroll(function () {
        if ($(this).scrollTop() > 20) {
            $("#scroll-to-top").fadeIn(500);
        } else {
            $("#scroll-to-top").fadeOut(500);
        }
    });

    // Scroll gently to top if scroll to top arrow is clicked
    $("#scroll-to-top").click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 500);
    });

});


function showAjaxLoader() {
    //find ajax loader div tag
    var loaderDiv = $("#__AjaxLoader");
    if (loaderDiv.length === 0) {
        //create ajax loader div tag, if not present
        loaderDiv = $("<div />;")
        .attr("id", "__AjaxLoader")
        .css("position", "absolute")
        .css("display", "block")
        .css("z-index", "10000")
        .addClass("ajaxLoader");
        loaderDiv.appendTo("body");
    }

    //center ajax loader div tag in the browser window
    var doc = $(document);
    loaderDiv.css('top', (doc.height() - loaderDiv.height()) / 2);
    loaderDiv.css('left', (doc.width() - loaderDiv.width()) / 2);

    //show it
    loaderDiv.show();
}

function hideAjaxLoader() {
    //hide ajax loader div tag, if present
    $("#__AjaxLoader").hide();
}

function trimText(text, allowedlength) {

    if (text == null || text == "") return "";

    text = text.replace(/,\s*$/, "");

    if (text.length > allowedlength) {
        return text.substring(0, allowedlength - 3) + "...";
    }
    return text;
}

function fixedEncodeURIComponent(str) {
    return encodeURIComponent(str).replace(/[!'()*]/g, escape);
}

Date.prototype.isValid = function () {
    // An invalid date object returns NaN for getTime() and NaN is the only object not strictly equal to itself.
    return this.getTime() === this.getTime();
};