// When client is not on same domain.
//var serviceUrl = 'http://localhost33319/api/ProductsAPI/';

// When client is local.
var serviceUrl = './api/ProductsAPI';

// http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
function sendRequest() {
    $("#products").replaceWith("<span id='value1'></span>");
    var method = $('#method').val();
    $.ajax({
        type: method,
        url: serviceUrl
    }).done(function (data) {
        data.forEach(function (val) {
            callback(val)
        });
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $('#value1').text(jqXHR.responseText || textStatus);
    });
}

function callback(val) {
    //  $("#manufacturers").replaceWith("<span id='value1'>(Result)</span>");
    $("#value1").replaceWith("<ul id='products' />");
    var str = "Product ID: " + val.ProductId + " Description: " + val.ProductDescription + " Manufacturer: " + val.Manufacturer;
    $('<li/>', { text: str }).appendTo($('#products'));
}

// Deletes and refreshes list.
function updateList() {
    $("#products").replaceWith("<span id='value1'>(Result)<br /></span>");
    sendRequest();
}
function find() {
    var id = $('#productIdFind').val();
    $.getJSON(serviceUrl + "/" + id,
        function (data) {
            if (data == null) {
                $('#productFind').text('Product not found.');
            }
            var str = data.ProductName + ': ' + data.ProductDescription;
            $('#productFind').text(str);
        })
    .fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#productFind').text('Find error: ' + err);
        });
}


// Add a new manufacturer.
function create() {
    jQuery.support.cors = true;
    var product = {
        ID: 6,
        Description: $('#txtAdd_description').val(),
        Manufacturer: $('#txtAdd_manufacturer').val(),
        Price: $('#txtAdd_price').val()
    };
    //product.ID++;
    var id = $('#productIdFind').val();

    var cr = JSON.stringify(product);
    $.ajax({
        url: serviceUrl,
        type: 'POST',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#productCreate')
                .text('Product successfully created.');
            updateList();
        },
        error: function (_httpRequest, _status, _httpError) {
            // XMLHttpRequest, textStatus, errorThrow
            $('#productCreate')
            .text('Error while adding product.  XMLHttpRequest:'
                    + _httpRequest + '  Status: ' + _status
                    + '  Http Error: ' + _httpError);
        }
    });
}


// Update a manufacturer object.
function update() {
    jQuery.support.cors = true;
    var product = {
        ID: $('#txtUpdate_Id').val(),
        Description: $('#txtUpdate_description').val(),
        Manufacturer: $('#txtUpdate_manufacturer').val(),
        Price: $('#txtUpdate_price').val()
    };

    var cr = JSON.stringify(product);
    $.ajax({

        url: serviceUrl + "/" + $('#txtUpdate_Id').val(),
        type: 'PUT',
        data: JSON.stringify(product),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#productUpdate')
            .text('The update was successful.');
            updateList();
        },
        error: function (_httpRequest, _status, _httpError) {
            $('#productUpdate')
            .text('Error while adding product.  XMLHttpRequest:'
            + _httpRequest + '  Status: ' + _status + '  Http Error: '
            + _httpError);
        }
    });
}

function del() {
    var id = $('#mfgID').val();
    $.ajax({
        url: serviceUrl + "/" + id,
        type: 'DELETE',
        dataType: 'json',

        success: function (data) {
            $('#productDelete').text('Delete successful.');
            updateList();
        }
    }).fail(
        function (jqueryHeaderRequest, textStatus, err) {
            $('#productDelete').text('Delete error: ' + err);
        });
}

