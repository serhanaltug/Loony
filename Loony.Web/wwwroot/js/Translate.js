function Translate(key) {
    var result;
    $.ajax({
        type: "GET",
        async: false,
        url: '/Translation/Translate',
        dataType: 'text',
        data: { key: key },
        success: function (data) {
            result = data;
        }
    });
    return result;
}