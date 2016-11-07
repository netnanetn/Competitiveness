var mobile = false;
var _defaultTextBox = 'Nhập tên doanh nghiệp/cửa hàng của bạn ...';
var _defaultRegistrationAddress = 'http://reg.bizwebvietnam.com/SiteRegistration.aspx'
var IsOpenLogin = false;
var IOpenCTVPopup = false;
var IOpenSEOMaster = false;
///////////////////////////////////////////////////////////////////////////////////////////////////
// Hàm dùng cho mobile
$(function () {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        mobile = true;
        $("head").append("<link type='text/css' rel='stylesheet' href='/Themes/Portal/Default/Styles/mobile.css?4.0.28' />");
        setScaleImg();
    }
});

var scaleImg = function (img) {
    var sWidth = $(window).width();
    var ratio = img.width / img.height;
    if (sWidth >= img.width) return;
    else {
        $(img).removeAttr("style");
        img.width = sWidth;
    }
}

function setScaleImg() {
    $('#wrapper img').load(function () { scaleImg(this); });
}

///////////////////////////////////////////////////////////////////////////////////////////////////
// Hàm dùng chung

// Get dữ liệu từ cookie
function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) { return unescape(y); }
    }
}

// Lưu dữ liệu vào cookie
function setCookie(c_name, value, minutes) {
    // do something
    // ...
}
function setCookie(c_name, value) {
    $.cookie(c_name, value, { path: '/', expires: 30 });
}

// Xóa cookie
function removeCookie(c_name) {
    document.cookie = c_name + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
}

// Validate Email
function isValidEmail(email) { 
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

// Allow number input
function keypress(e) {
    var keypressed = null;
    if (window.event) { keypressed = window.event.keyCode; }
    else { keypressed = e.which; }
    if (keypressed < 48 || keypressed > 57) {
        if (keypressed == 8 || keypressed == 127 || keypressed == 0) { return; }
        return false;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
// Đăng ký dùng thử dịch vụ
$(function () {
    var username = '', refer = '', gclid = '', utm_source = '';
    var date = new Date();
    date.setTime(date.getTime() + (10 * 60 * 1000)); // thời gian set cookie tham số gclid và utm_source (10 mins)

    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == "kd") { username = pair[1]; }
        if (pair[0] == "ref") { refer = pair[1]; }
        if (pair[0] == "gclid") { gclid = pair[1]; }
        if (pair[0] == "utm_source") { utm_source = pair[1]; }
    }
    if (username != '') setCookie("kd", username);
    if (refer != '') setCookie("ref", refer);
    if (gclid != '') {
        $.cookie('gclid', gclid, { expires: date });
    }
    if (utm_source != '') {
        $.cookie('utm_source', utm_source, { expires: date });
    }

    // focus vào 2 textbox [banner, bottom]
    $("#site_name_banner, #site_name_bottom").attr('placeholder', _defaultTextBox);
    $('#site_name_banner').focus(function () {
        $('.registration-form .arrow').fadeIn(300);
    });
    $('#site_name_banner').focusout(function () {
        $('.registration-form .arrow').fadeOut(100);
    });

    // đăng ký fancybox cho trigger link
    $('.trigger-registration').fancybox({
        width: '847px',
        height: '650px',
        autoSize: true,
        fitToView: false,
        openEffect: 'fade',
        closeEffect: 'fade',
        closeClick: false, // prevents closing when clicking INSIDE fancybox
        helpers: {
            overlay: {
                closeClick: false
            } // prevents closing when clicking OUTSIDE fancybox
        },
        keys: {
            close: null
        },
        'beforeShow': function () {
            $('.fancybox-skin').addClass('fancybox-skin-reg');
        },
        'afterShow': function () {
            $('.fancybox-item.fancybox-close').addClass('fancybox-close-reg');
            if (mobile == false) {
                $('.fancybox-item.fancybox-close').addClass('fancybox-close-reg-web');
            }
        },
        'afterClose': function () {
            $('.fancybox-skin').removeClass('fancybox-skin-reg');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-reg');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-reg-web');
        }
    });

    // kích đăng ký dùng thử trên banner home
    $('.banner-home-registration').click(function () {
        var href = _defaultRegistrationAddress;
        var sitename = $("#site_name_banner").val();
        if (sitename != '' && sitename != _defaultTextBox) {
            href = href + '?sitename=' + sitename;
        }

        if (getCookie('gclid') == null && getCookie('utm_source') == null) {
            if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
        }
        if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
        if (getCookie('gclid') != null) href = href + '&gclid=' + getCookie('gclid');
        if (getCookie('utm_source') != null) href = href + '&utm_source=' + getCookie('utm_source');
        if (href.indexOf('.aspx&') > 0) href = href.replace('.aspx&', '.aspx?');

        $('#registrationIFrame iframe').attr('src', href);
        $('.trigger-registration').trigger('click');
        return false;
    });

    // kích đăng ký dùng thử trên header
    $('.header-registration, .price-registration').click(function () {
        var href = _defaultRegistrationAddress;
        var param = '';

        if (getCookie('gclid') == null && getCookie('utm_source') == null) {
            if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
        }
        if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
        if (getCookie('gclid') != null) href = href + '&gclid=' + getCookie('gclid');
        if (getCookie('utm_source') != null) href = href + '&utm_source=' + getCookie('utm_source');
        if (href.indexOf('.aspx&') > 0) href = href.replace('.aspx&', '.aspx?');

        $('#registrationIFrame iframe').attr('src', href);
        $('.trigger-registration').trigger('click');
        return false;
    });

    // kích đăng ký dùng thử ở control bottom
    $('.site_registration_bottom').click(function () {
        var href = _defaultRegistrationAddress;

        var sitename = $("#site_name_bottom").val();
        if (sitename != '' && sitename != _defaultTextBox) {
            href = href + '?sitename=' + sitename;
        }

        if (getCookie('gclid') == null && getCookie('utm_source') == null) {
            if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
        }
        if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
        if (getCookie('gclid') != null) href = href + '&gclid=' + getCookie('gclid');
        if (getCookie('utm_source') != null) href = href + '&utm_source=' + getCookie('utm_source');
        if (href.indexOf('.aspx&') > 0) href = href.replace('.aspx&', '.aspx?');

        $('#registrationIFrame iframe').attr('src', href);
        $('.trigger-registration').trigger('click');
        return false;
    });

    // đăng nhập website từ trang chủ
    $(".action-links .login").fancybox({
        autoSize: true,
        openEffect: 'fade',
        closeEffect: 'fade',
        fitToView: false,
        'beforeShow': function () {
            $('.fancybox-skin').addClass('fancybox-skin-regname');
        },
        'afterShow': function () {
            $('.fancybox-item.fancybox-close').addClass('fancybox-close-request');
            IsOpenLogin = true;
        },
        'afterClose': function () {
            $('.fancybox-skin').removeClass('fancybox-skin-request');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-request');
            IsOpenLogin = false;
        }
    });

    $(".trigger-logincreatesite").fancybox({
        autoSize: true,
        openEffect: 'fade',
        closeEffect: 'fade',
        fitToView: false,
        'beforeShow': function () {
            $('.fancybox-skin').addClass('fancybox-skin-regname');
            $('#webdemo, #sysUsername, #sysPassword').val("");
            $('#sysUsername').focus();
        },
        'afterShow': function () {
            $('.fancybox-item.fancybox-close').addClass('fancybox-close-request');
        },
        'afterClose': function () {
            $('.fancybox-skin').removeClass('fancybox-skin-request');
            $('.fancybox-item.fancybox-close').removeClass('fancybox-close-request');
        }
    });
});

// đăng ký dùng thử từ module skins
function RegWithSkin(skinId) {
    var href = _defaultRegistrationAddress + '?skinid=' + skinId;
    if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
    if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
    $('#registrationIFrame iframe').attr('src', href);
    $('.trigger-registration').trigger('click');
    return false;
}

// đăng ký dùng thử kho giao diện cao cấp
function SeniorReg(siteid) {
    var href = _defaultRegistrationAddress + '?siteid=' + siteid;
    //if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
    //if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
    $('#registrationIFrame iframe').attr('src', href);
    $(".trigger-logincreatesite").trigger('click');
}

///////////////////////////////////////////////////////////////////////////////////////////////////
// Popup đăng nhập
$(function () {
    $(document).keypress(function (e) {
        if (e.which == 13) {
            if (IsOpenLogin) {
                $("#confirmPopupLogin").click();
            } else if (IOpenCTVPopup) {
                $(".popup-agent input[type='submit']").click();
            } else if (IOpenSEOMaster) {
                $("input[type='submit'][step='b1']").click();
            }
        }
    });
    $("#site_name_banner").keypress(function (e) {
        if (e.which == 13) {
            $('.banner-home-registration').trigger('click');
        }
    });
    $("#site_name_bottom").keypress(function (e) {
        if (e.which == 13) {
            $('.site_registration_bottom').trigger('click');
        }
    });
    $("#confirmPopupLogin").click(function () {
        if (CheckValidate() == false) return false;

        var FeedCrd = {};
        FeedCrd["username"] = $("#usernameBox").val();
        FeedCrd["password"] = $("#passwordBox").val();
        FeedCrd["website"] = $("#addressBox").val().replace("http://", "");

        $.ajax({
            type: "POST",
            url: "/Handlers/RemoteLogin.ashx?q=" + JSON.stringify(FeedCrd),
            data: JSON.stringify(FeedCrd),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.Code == 0) {
                    window.location = "http://" + $("#addressBox").val() + "/admin.aspx?module=dashboard&token=" + response.ErrorCode + "&username=" + $("#usernameBox").val();
                }
                else {
                    alert(response.ErrorCode);
                }
            },
            failure: function (msg) { console.log(msg); }
        });
        return false;
    });

    // Đăng nhập tài khoản hệ thống để tạo site với kho giao diện cao cấp
    $("#loginForCreateSiteContainer a").click(function () {
        if ($("#sysUsername").val() == "") {
            alert("Vui lòng nhập Tên đăng nhập tài khoản của bạn!");
            $("#sysUsername").focus();
            return false;
        }
        if ($("#sysPassword").val() == "") {
            alert("Vui lòng nhập Mật khẩu tài khoản của bạn!");
            $("#sysPassword").focus();
            return false;
        }

        var FeedCrd = {};
        FeedCrd["username"] = $("#sysUsername").val();
        FeedCrd["password"] = $("#sysPassword").val();
        FeedCrd["webdemo"] = $("#webdemo").val();
        $.ajax({
            type: "POST",
            url: "/Handlers/LoginForCreateSite.ashx?q=" + JSON.stringify(FeedCrd),
            data: JSON.stringify(FeedCrd),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.Code == 0) {
                    var href = $('#registrationIFrame iframe').attr('src');
                    href = href + '&token=' + response.ErrorCode;
                    href = href + "&kd=" + $("#sysUsername").val();
                    $('#registrationIFrame iframe').attr('src', href);
                    $('.trigger-registration').trigger('click');
                }
                else {
                    alert(response.ErrorCode);
                }
            },
            failure: function (msg) { console.log(msg); }
        });

        return false;
    });

    $("#sysUsername, #sysPassword").keypress(function (e) {
        if (e.which == 13) {
            $("#loginForCreateSiteContainer a").trigger("click");
        }
    });
});

function CheckValidate() {
    if ($("#addressBox").val() == "") {
        alert("Vui lòng nhập Địa chỉ Website của bạn!");
        $("#addressBox").focus();
        return false;
    }
    else {
        var temp = $("#addressBox").val();
        if ($("#addressBox").val().indexOf("http://") == -1) {
            temp = "http://" + $("#addressBox").val();
        }
        if (ValidURL(temp) == false) {
            alert("Địa chỉ Website không hợp lệ!");
            $("#addressBox").focus();
            return false;
        }
    }
    if ($("#usernameBox").val() == "") {
        alert("Vui lòng nhập Tên đăng nhập Website của bạn!");
        $("#usernameBox").focus();
        return false;
    }
    if ($("#passwordBox").val() == "") {
        alert("Vui lòng nhập Mật khẩu truy cập Website của bạn!");
        $("#passwordBox").focus();
        return false;
    }
    return true;
}

function ValidURL(url) {
    var theUrl = url;
    if (theUrl.match(/^(http|ftp)\:\/\/\w+([\.\-]\w+)*\.\w{2,4}(\:\d+)*([\/\.\-\?\&\%\#]\w+)*\/?$/i))
        return true;
    else
        return false;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
// Đăng ký nhận bản tin khuyến mại
$(function () {
    $('#bt_subscription').click(function () {
        if ($('#tb_subscription').val() == '') {
            alert('Vui lòng nhập địa chỉ Email đăng ký nhận bản tin!');
            $('#tb_subscription').focus();
            return false;
        }
        else {
            if (isValidEmail($('#tb_subscription').val()) == false) {
                alert('Địa chỉ Email không hợp lệ!');
                $('#tb_subscription').focus();
                return false;
            }
        }
        return true;
    });
    $('#tb_subscription').keypress(function (e) {
        if (e.which == 13) {
            $('#bt_subscription').trigger('click');
        }
    });
});

function appendAdwordCompleted(siteId, siteName)
{
    var js = 
        "<script>" +
            "var _gaq = _gaq || [];" +
            "_gaq.push(['_setAccount', 'UA-15502859-4']);" +
            "_gaq.push(['_trackPageview']);" +
            "_gaq.push(['_addTrans', '" + siteId + "', 'Bizweb', '0', '0', '0', '', '', 'VNM']);" +
            "_gaq.push(['_addItem', '" + siteId + "', 'Free Trial', '" + siteName + ".bizwebvietnam.com', 'Bizweb', '0', '1']);" +
            "_gaq.push(['_trackTrans']);" +
            "(function () {" +
                "var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;" +
                "ga.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'google-analytics.com/ga.js';" +
                "var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);" +
            "})();" +
        "</script>";
    $('head').append(js);
}

function popupRegistration() {
    var href = _defaultRegistrationAddress;
    var sitename = $("#input-video").val();
    if (sitename != '' && sitename != _defaultTextBox) {
        href = href + '?sitename=' + sitename;
    }

    if (getCookie('gclid') == null && getCookie('utm_source') == null) {
        if (getCookie('kd') != null) href = href + '&kd=' + getCookie('kd');
    }
    if (getCookie('ref') != null) href = href + '&ref=' + getCookie('ref');
    if (getCookie('gclid') != null) href = href + '&gclid=' + getCookie('gclid');
    if (getCookie('utm_source') != null) href = href + '&utm_source=' + getCookie('utm_source');
    if (href.indexOf('.aspx&') > 0) href = href.replace('.aspx&', '.aspx?');

    $('#registrationIFrame iframe').attr('src', href);

    $('#html5-close').trigger('click');
    $('.trigger-registration').trigger('click');
}