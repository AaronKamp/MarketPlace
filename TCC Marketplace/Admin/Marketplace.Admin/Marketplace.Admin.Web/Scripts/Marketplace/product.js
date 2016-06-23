/// <reference path="../site.js" />
/*Products*/
$('#ProductCategories').change(function () {
    var catId = this.value;
    if (catId > 0) {
        populateProducts(catId);
    }
    hideAjaxLoader();
});

$('#btnAddProduct').click(function () {

    if ($('#ProductCategories').val() == 0) {
        alert('Select a Product Category');
        $('#ProductCategories').focus();
        return false;
    }

    if ($('#dvProducts input:checked').length == 0) {
        alert('Select a Product Code');
        return false;
    }

    var product = {};
    product.CategoryId = $('#ProductCategories').val();
    product.CategoryName = $('#ProductCategories :selected').text();

    product.ProductIds = [];
    product.ProductNames = ''
    $('#dvProducts input:checked').each(function () {
        product.ProductIds.push($(this).val());
    });
    $('#dvProducts input:checked').each(function () {
        product.ProductNames += ($(this).attr('name')) + ', ';
    });

    BindProducts(product);
    $('#product-pops').hide();
});

var BindProducts = function (product) {
    var tblProd = $('#tblProducts');

    if ($("#trProduct" + product.CategoryId).length > 0) {
        $("#trProduct" + product.CategoryId).remove();
    }

    var categoryJString = fixedEncodeURIComponent(JSON.stringify(product));
    var newRow = "<tr id='trProduct" + product.CategoryId + "'>  <input type='hidden' value='" + categoryJString + "' name='Product' />"
                        + '<td>' + product.CategoryName + '</td>'
                        + '<td><span class="trim-column" style="width:325px">' + product.ProductNames + '</span></td>'
                        + '<td align="center"><a href="#" name="editProduct"> <img src="/Content/images/edit.png"> </a></td>'
                        + '<td align="center"><a href="#" name="deleteProduct"><img src="/Content/images/delete.png"> </a></td>'
                   + '</tr>'
    tblProd.append(newRow);
}

$(document).on('click', "#tblProducts tr td a[name='editProduct']", function (e) {

    e.preventDefault();
    $('#btnAddProduct').val('Update');
    var jsonString = $(this).closest('td').closest('tr').find('input[type="hidden"][name="Product"]').val();
    var product = JSON.parse(decodeURIComponent(jsonString));
    BindProductPopUp(product);
});

$(document).on('click', "#tblProducts tr td a[name='deleteProduct']", function (e) {
    e.preventDefault();
    $(this).closest('td').closest('tr').remove();
});

var BindProductPopUp = function (product) {
    $('#product-pops').show();
    showAjaxLoader();
    $('#ProductCategories').val(product.CategoryId);
    populateProducts(product.CategoryId);

    $('#ProductCategories').attr("disabled", "disabled");

    $.each(product.ProductIds, function (index, value) {
        $('#chkProduct' + value).attr('checked', true);
    });
    hideAjaxLoader();

};

var populateProducts = function (catId) {
    var Products_div = $('#dvProducts');
    Products_div.empty();
    var productCategory = $("#ProductCategories option:selected").text();
    showAjaxLoader();
    $.ajax({
        url: "/Services/GetProducts",//"@Url.Action("GetProducts", "Services")",
        data: { catId: catId },
        dataType: "json",
        type: "GET"
    })
    .done(function (data) {
        if (data.length == 0) {
            $("#dvProducts").html("<span class='text-danger'>No products found for the category '" + productCategory + "' </span>");
            $("#btnAddProduct").attr("disabled", true);
        }
        else {
            $("#btnAddProduct").removeAttr("disabled");

            $.each(data, function (i) {
                var checkboxhtml = '<label><input type="checkbox" value="' +
                                    data[i].Value + '" name="' + data[i].Text + '" id="chkProduct' + data[i].Value + '"/>&nbsp;&nbsp;' + data[i].Text + '</label>';
                Products_div.append(checkboxhtml);
            });

            var productString = $("#trProduct" + catId).find('input[type="hidden"][name="Product"]');
            if (productString.length > 0) {
                var jsonString = productString.val();
                var product = JSON.parse(decodeURIComponent(jsonString));
                $.each(product.ProductIds, function (index, value) {
                    $('#chkProduct' + value).attr('checked', true);
                });

            }
            else {
                $('#dvProducts :checkbox:enabled').prop('checked', true);
            }
        }
    })
    .fail(function () {
        alert("Unable to populate Categories.");
    })
    .always(function () {
        hideAjaxLoader();
    });
};
/*Products*/