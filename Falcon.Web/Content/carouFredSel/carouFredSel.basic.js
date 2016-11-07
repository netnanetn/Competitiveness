var interval;
var transition_time = 1000;
var interval_time = 20000;

$(function () {
    slideShow();

    $("#banner-control a").each(function () {
        $(this).click(function () {
            var pos = $(this).attr("pos"); // vị trí mới

            var obj = $("#banner-slide img[pos='" + pos + "']");
            var current = ($('#banner-slide img.show') ? $('#banner-slide img.show') : $('#banner-slide img:first'));
            var current_pos = $(current).attr('pos');

            if(pos == current_pos) return;

            var next = ((obj.length) ? ((obj.hasClass('caption')) ? $('#banner-slide img:first') : obj) : $('#banner-slide img:first'));
            next.css({ opacity: 0.0 }).removeClass('hidden').addClass('show').animate({ opacity: 1.0 }, transition_time);
            current.animate({ opacity: 0.0 }, transition_time).removeClass('show').addClass('hidden');
            $(current).css('z-index', '-1');
            $(next).css('z-index', '1');
            set_background($(next));

            var current_c = current;

            var obj_text = $("#banner-text .banner-text-item[pos='" + pos + "']");
            current = ($('#banner-text .banner-text-item.show') ? $('#banner-text .banner-text-item.show') : $('#banner-text .banner-text-item:first'));
            next = ((obj_text.length) ? ((obj_text.hasClass('caption')) ? $('#banner-text .banner-text-item:first') : obj_text) : $('#banner-text .banner-text-item:first'));
            next.css({ opacity: 0.0 }).removeClass('hidden').addClass('show').animate({ opacity: 1.0 }, transition_time);
            current.animate({ opacity: 0.0 }, transition_time).removeClass('show').addClass('hidden');
            $(current).css('z-index', '-1');
            $(next).css('z-index', '1');

            if (pos <= 1) { $('.km-icon').fadeIn(); }
            else { $('.km-icon').fadeOut(); }

            var current_pos = $(current_c).attr("pos");
            $("#banner-control a[pos='" + current_pos + "']").removeClass('show');
            $("#banner-control a[pos='" + pos + "']").addClass('show');

            clearInterval(interval);
            interval = setInterval('gallery()', interval_time);
        });
    });
});
function slideShow() {
    $('#banner-text .banner-text-item').css({ opacity: 0.0 });
    $('#banner-text .banner-text-item:first').css({ opacity: 1.0 });

    interval = setInterval('gallery()', interval_time);
}

function gallery() {
    var current = ($('#banner-slide img.show') ? $('#banner-slide img.show') : $('#banner-slide img:first'));
    var next = ((current.next().length) ? ((current.next().hasClass('caption')) ? $('#banner-slide img:first') : current.next()) : $('#banner-slide img:first'));
    next.css({ opacity: 0.0 }).removeClass('hidden').addClass('show').animate({ opacity: 1.0 }, transition_time);
    current.animate({ opacity: 0.0 }, transition_time).removeClass('show').addClass('hidden');
    $(current).css('z-index', '-1');
    $(next).css('z-index', '1');
    set_background($(next));

    var current_c = current, next_c = next;

    current = ($('#banner-text .banner-text-item.show') ? $('#banner-text .banner-text-item.show') : $('#banner-text .banner-text-item:first'));
    next = ((current.next().length) ? ((current.next().hasClass('caption')) ? $('#banner-text .banner-text-item:first') : current.next()) : $('#banner-text .banner-text-item:first'));
    next.css({ opacity: 0.0 }).removeClass('hidden').addClass('show').animate({ opacity: 1.0 }, transition_time);
    current.animate({ opacity: 0.0 }, transition_time).removeClass('show').addClass('hidden');

    $(current).css('z-index', '-1');
    $(next).css('z-index', '1');

    var current_pos = $(current_c).attr("pos");
    var next_pos = $(next_c).attr("pos");
    $("#banner-control a[pos='" + current_pos + "']").removeClass('show');
    $("#banner-control a[pos='" + next_pos + "']").addClass('show');

    if (next_pos <= 1) { $('.km-icon').fadeIn(); }
    else { $('.km-icon').fadeOut(); }
}

function set_background(img)
{
    var image = $(img).attr('src');
    $('#banner-slide').animate({ opacity: 0.1 }, 'medium', function () {
        $(this).css({ 'background-image': 'url(' + image + ')' }).animate({ opacity: 1.0 });
    });
}