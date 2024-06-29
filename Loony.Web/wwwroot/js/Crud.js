$(document).on('click', '.btnNew', function (e) {
    $("#New").toggle();
});

$(document).on('click', '.btnEdit', function (e) {
    var row = $(this).closest("tr");
    var Id = $(this).attr('id');

    $.ajax({
        type: 'GET',
        url: window.location.href + '/_Edit',
        data: { id: Id },
        cache: false,
        success: function (data) {
            row.hide();
            row.after(data);
        },
        error: function () { toastr.error(Translate('msgEditError')); }
    });


});

$(document).on('click', '.btnCancel', function (e) {
    var row = $(this).closest("tr").closest("table").closest("tr");
    if (!row.length) row = $(this).closest("tr");
    row.prev().show();
    row.remove();
});

$(document).on('click', '.btnSave', function (e) {
    e.preventDefault();
    var row = $(this).closest("tr").closest("table").closest("tr");
    if (!row.length) row = $(this).closest("tr");
    var form = $(this).closest("form");

    $.ajax({
        type: 'POST',
        url: window.location.href + '/_Edit',
        data: form.serializefiles(),
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok)
                GetData(data.page, data.order, data.sortdir, data.filters, data.search, data.type);
            row.after(data);
            row.remove();
            toastr.info(Translate('msgSaved'));
        },
        error: function () {
            toastr.error(Translate('msgSaveError'));
        }
    });


});

$(document).on('click', '.btnDelete', function (e) {
    if (confirm(Translate('msgDelete'))) {
        var row = $(this).closest("tr");
        var Id = $(this).attr('id');

        $.ajax({
            type: 'GET',
            url: window.location.href + '/_Delete',
            data: { id: Id },
            cache: false,
            success: function (data) {
                console.log(data);
                if (data.ok)
                    GetData(data.page, data.order, data.sortdir, data.filters, data.search, data.type);
                row.remove();
                toastr.info(Translate('msgDeleted'));
            },
            error: function () { toastr.error(Translate('msgDeleteError')); }
        });
    };
});

$.getScript("/js/Translate.js");

//USAGE: $("#form").serializefiles();
(function ($) {
    $.fn.serializefiles = function () {
        var obj = $(this);
        var formData = new FormData();
        $.each($(obj).find("input[type='file']"), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });
        var params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });
        return formData;
    };
})(jQuery);