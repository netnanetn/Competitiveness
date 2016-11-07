$(function () {
    setCapcha_Free();
    $('#reset_Free').click(function () {
        setCapcha_Free();
    });
    $("#tb_domain, #cb_domain_ext").change(function () {
        if ($("#tb_domain").val() == "") return;
        $("#notifyDomain").hide();
        $("#checkingDomain").slideDown();
        $("#checkingDomain strong").html("www." + $("#tb_domain").val() + $("#cb_domain_ext").val());
        $.ajax({
            type: "GET",
            dataType: "text",
            data: "domain=" + $("#tb_domain").val() + $("#cb_domain_ext").val(),
            url: "/CheckDomain.ashx",
            success: function (response) {
                if (response == "False") {
                    $("#notifyDomain").html("Chúc mừng bạn, bạn có thể đăng ký miễn phí tên miền <strong>'" + $("#tb_domain").val() + $("#cb_domain_ext").val() + "'</strong>.");
                }
                else {
                    $("#notifyDomain").html("Tên miền <strong>'" + $("#tb_domain").val() + $("#cb_domain_ext").val() + "'</strong> đã được đăng ký.<br />Bạn vui lòng chọn tên miền khác để đăng ký miễn phí.");
                }
                $("#checkingDomain").slideUp();
                $("#notifyDomain").slideDown();
            },
            failure: function (msg) { alert(msg); }
        });
    });
});
function setCapcha_Free() {
    $('#imgCapcha_Free').attr("src", "/Ajax/Capcha/ShowCaptchaImage?a=" + Math.random() + "&f=2");
}
function CheckValidate() {
    if ($("#tb_domain").val() == "") {
        alert("Vui lòng nhập thông tin tên miền đăng ký");
        $("#tb_domain").focus();
        return false;
    }
    if ($("#FullName").val() == "") {
        alert("Vui lòng nhập thông tin Người đăng ký");
        $("#FullName").focus();
        return false;
    }
    if ($("#Email").val() == "") {
        alert("Vui lòng nhập thông tin Email");
        $("#Email").focus();
        return false;
    }
    else {
        if (isValidEmail($("#Email").val()) == false) {
            alert("Địa chỉ Email không hợp lệ");
            $("#Email").focus();
            return false;
        }
    }
    if ($("#PhoneNumber").val() == "") {
        alert("Vui lòng nhập thông tin Điện thoại liên hệ");
        $("#PhoneNumber").focus();
        return false;
    }
    if ($("#Address").val() == "") {
        alert("Vui lòng nhập thông tin Địa chỉ liên lạc");
        $("#Address").focus();
        return false;
    }
    if ($("#Capcha").val() == "") {
        alert("Vui lòng nhập thông tin Mã an toàn");
        $("#Capcha").focus();
        return false;
    }
    return true;
}