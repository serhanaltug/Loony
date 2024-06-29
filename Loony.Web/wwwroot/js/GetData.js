function submitForm() {
    $("#filterForm").submit();
}

$("#filterForm").submit(function (e) {
    e.preventDefault();

    var form = $(this);

    var searchBy = $("#searchBy").val();

    var filtersToSend = new Array();
    var filters = document.getElementsByName('filter[]');

    for (key = 0; key < filters.length; key++) {
        if (filters[key].value != '') filtersToSend.push(filters[key].id + ':' + filters[key].value);
    }

    GetData(1, null, null, encodeURI(JSON.stringify(filtersToSend)), searchBy);
});

var url = '/_List';
var type = '_List';

function GetData(page, order, sortdir, filters, search) {
    $.ajax({
        type: "GET",
        url: window.location.href + url,
        data: { page: page, order: order, sortdir: sortdir, filters: filters, search: search, type: type },
        success: function (data) {
            $("#gridView").html(data);
        }
    });
}

$(function () {
    GetData();
});