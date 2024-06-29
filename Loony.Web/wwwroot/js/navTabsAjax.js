$('.nav-tabs a').click(function (e) {
    e.preventDefault()
    if ($(this).data('loaded') != 1) {

        var tab = $(this).attr("href");
        var url = $(this).attr('data-url');

        if (typeof (url) != 'undefined') {
            $(tab).load(url, function (data) {
                $(this).data('loaded', 1);
                $(this).tab('show')
            });
        }
    }
})

if (location.hash) {
    var tabid = location.hash.substr(1);
    $('a[href="#' + tabid + '"]').parents('.tab-pane:hidden').each(function () {
        var tabid = $(this).attr("id");
        $('a[href="#' + tabid + '"]').click();
    });
    $('a[href="#' + tabid + '"]').click();
}
