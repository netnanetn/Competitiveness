var mobile;

$(function () {
    if (getCookie('support_st') != null) {
        $('.bg-support').hide();
        var support_st = getCookie('support_st');
        if (support_st == 'collapse') {
            $('.support-dock').removeClass('hidden').animate({ opacity: 1 }, 1);
        }
        if (support_st == 'expand') {
            $('.support-dock').removeClass('hidden bottom').addClass('top').animate({ opacity: 1 }, 1);

            var container_width = $(window).width() - 219;
            if (mobile == false) $("#container").css("width", container_width + 'px');
            if (container_width < 1280) $('#prev, #next').hide();

            $('#collapse').show();
            $("#expand").hide();
        }
    }

    $('.bg-support').delay(500).animate({ opacity: 1, right: 0 }, 1200); // di chuyển ảnh TràPTH từ ngoài vào góc dưới phải
    $('.bg-support .minimize').click(function () { // click nút thu nhỏ (ảnh TràPTH)
        setCookie('support_st', 'collapse');
        $('.bg-support').animate({ opacity: 0, bottom: -200 }, 500, function () {
            $('.support-dock').removeClass('hidden').animate({ opacity: 1 }, 500); // hiển thị khung hỗ trợ nhỏ
        }); // ẩn ảnh TràPTH
    });
    $('.bg-support .maximize').click(function () { // click nút hiển thị danh sách tư vấn viên
        $('.bg-support').animate({ opacity: 0, bottom: -200 }, 500, function () {
            ExpandDock();
        }); // ẩn ảnh TràPTH
    });
    $(".support-dock .show").click(function () { // kích vào khung tư vấn viên
        var expand = $("#expand").css("display"); // lấy trạng thái hiển thị của nút expand
        if (expand == "" || expand == "block") ExpandDock();
        else CollapseDock();
        return false;
    });
    
    $('#persons').css('height', screen.height + 'px');

    window.onresize = function () {
        var _class = $('.support-dock').attr('class');
        var _display = $('.support-dock #persons').css('display');
        if (_class.indexOf('hidden') != -1) {
            $("#container").css("width", '100%');
        }
        else {
            if (_display == 'block') {
                var container_width = ($(window).width() - 219);
                if (mobile == false) $("#container").css("width", container_width + 'px');
            }
            else {
                $("#container").css("width", '100%');
            }
        }
        $('#persons').css('height', $(window).height() + 'px');
    };

    var width = $(window).width(), height = $(window).height();
    var interval_window_resize = setInterval(function () {
        if ($(window).width() != width || $(window).height() != height) {
            $(window).trigger('resize');
        }
    }, 1000);
});
function CollapseDock() {
    $('.support-dock').removeClass('top').addClass('bottom');

    $("#container").css("width", '100%');
    if ($(document).width() > 1280) $('#prev, #next').show();

    $('#expand').show();
    $("#collapse").hide();

    setCookie('support_st', 'collapse');
}
function ExpandDock() {
    $('.support-dock').removeClass('hidden bottom').addClass('top').animate({ opacity: 1 }, 500);

    var container_width = $(window).width() - 219;
    if (mobile == false) $("#container").css("width", container_width + 'px');
    if (container_width < 1280) $('#prev, #next').hide();

    $('#collapse').show();
    $("#expand").hide();

    setCookie('support_st', 'expand');
}