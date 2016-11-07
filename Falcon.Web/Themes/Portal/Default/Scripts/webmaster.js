var mobile;

$(function () {
    var username = '';
    var check_cookie_username = 1;
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == "kd") { username = pair[1]; }
    }
    if (username != '') { setCookie("kd", username, 1); check_cookie_username = 0; }
    if (check_cookie_username == 1 && getCookie("kd") != null) { username = getCookie('kd'); }
    $('.register_webmaster, .register-webmaster').each(function () {
        var href = $(this).attr('href');
        href = href + '?kd=' + username;
        $(this).attr('href', href);
    });

    $(".register-webmaster, .register_webmaster").fancybox({
        width: '860px',
        height: '450px',
        fitToView: false,
        autoSize: true,
        openEffect: 'fade',
        closeEffect: 'fade',
        'beforeShow': function () {
            $('.fancybox-skin').addClass('fancybox-skin-webmaster');
        },
        'afterShow': function () {
            $('.fancybox-item.fancybox-close').addClass('fancybox-close-webmaster');
            if (mobile == false) {
                $('.fancybox-item.fancybox-close').addClass('fancybox-close-webmaster-web');
            }
        },
        'afterClose': function () {
            $('.fancybox-skin').removeClass('fancybox-skin-webmaster');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-webmaster');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-webmaster-web');
        }
    });
    $(".fb-thumb-1, .fb-thumb-2, .popupBanner").fancybox({
        fitToView: false,
        autoSize: true,
        openEffect: 'fade',
        closeEffect: 'fade'
    });
    $(".popupBanner").fancybox({
        width: '500px',
        height: '300px',
        openEffect: 'fade',
        closeEffect: 'fade'
    });
});

var clip = null;
function init() {
    clip = new ZeroClipboard.Client();
    clip.setHandCursor(true);
    clip.addEventListener('load', function (client) {
    });
    clip.addEventListener('mouseOver', function (client) {
        clip.setText(document.getElementById('fe_text').value);
    });
    clip.addEventListener('complete', function (client, text) {
        alert("Đã copy nội dung mã nhúng !");
    });
    clip.glue('d_clip_button', 'd_clip_container');
}