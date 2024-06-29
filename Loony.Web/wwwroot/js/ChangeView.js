$(document).on('click', '#changeView', function (e) {

    var btn = $(this).children("i");

    if (btn.hasClass('la-reorder')) {
        type = '_Grid';
        btn.removeClass("la-reorder");
        btn.addClass("la-image");
    }
    else {
        type = '_List';
        btn.removeClass("la-image");
        btn.addClass("la-reorder");
    }

    GetData();

});