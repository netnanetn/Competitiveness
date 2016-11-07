var imagePath = '/Themes/Portal/Default/Images/';
var res_container = '';

function OpenWin(domain, obj) {
    $('#whoisTrigger').trigger('click');
    $('#whois').html('<img alt="" src="' + imagePath + 'loading.gif" />');
    $.ajax({
        type: "GET",
        dataType: "text",
        data: "Domain=" + domain,
        url: "/WhoisDomain.ashx",
        success: function (response) {
            $('#whois').html(response);
        },
        failure: function (msg) { alert(msg); }
    });
}

function checkDomain() {
    var domain = $("#check_domain input[type='text']").val();

    if (domain != "") {
        $("#check_domain_result").slideDown(); // HIỆN FORM KẾT QUẢ

        if (res_container == '') res_container = $('#res_container').html();
        else $('#res_container').html(res_container);

        var checked = $("#check_domain input[type='checkbox']:checked").length; // CÁC TÊN MIỀN ĐƯỢC CHỌN
        var loading = '<img src="' + imagePath + 'loading.gif" alt="" />'; // LOADING ...

        var type = $(".domain-tabs a[class='current']").attr('type');

        if (checked == 0) // KHÔNG CHỌN TÊN MIỀN NÀO THÌ KIỂM TRA TẤT
        {
            $("#check_domain input[type='checkbox']").each(function () {
                var display = $(this).parent().css('display');
                var ext = $(this).val();
                var cb_result = $(".check-domain-result input[type='checkbox'][value='" + ext + "']"); // CHECKBOX Ở FORM KẾT QUẢ
                if (display != 'none') {
                    $(cb_result).parent().show();

                    $(cb_result).hide();
                    $(cb_result).parent().append('<img src="' + imagePath + 'loading.gif" alt="" class="loading" />');
                    $.ajax({
                        type: "GET",
                        dataType: "text",
                        data: "domain=" + domain + $(this).val(),
                        url: "/CheckDomain.ashx",
                        success: function (response) {
                            if (response != "False") {
                                var new_object =
                                    "<img class=\"nocheck\" src=\"" + imagePath + "nocheck.gif\" name=\"nocheck_" + ext + "\" alt=\"\" />" +
                                    "<span class=\"res\">" + ext + "</span>" +
                                    "<a target='_blank' class='inf' onclick='OpenWin(\"" + domain + ext + "\",this); return false;' href='javascript:;'> Xem thông tin</a>";
                                $(cb_result).parent().html(new_object);
                            }
                            else {
                                $(cb_result).show();
                                $(cb_result).parent().find('img').remove();
                            }
                        },
                        failure: function (msg) {
                            console.log(msg);
                        }
                    });
                }
                else {
                    $(cb_result).parent().hide();
                }
            });
        }
        else {
            $("#check_domain input[type='checkbox']").each(function () {
                var display = $(this).parent().css('display');
                var ext = $(this).val();
                var cb_result = $(".check-domain-result input[type='checkbox'][value='" + ext + "']"); // CHECKBOX Ở FORM KẾT QUẢ
                if (display != 'none') {
                    $(cb_result).parent().show();
                    if ($(this).prop('checked') == true) {
                        $(cb_result).hide();
                        $(cb_result).parent().append('<img src="' + imagePath + 'loading.gif" alt="" class="loading" />');

                        $.ajax({
                            type: "GET",
                            dataType: "text",
                            data: "domain=" + domain + $(this).val(),
                            url: "/CheckDomain.ashx",
                            success: function (response) {
                                if (response != "False") {
                                    var new_object =
                                        "<img class=\"nocheck\" src=\"" + imagePath + "nocheck.gif\" name=\"nocheck_" + ext + "\" alt=\"\" />" +
                                        "<span class=\"res\">" + ext + "</span>" +
                                        "<a target='_blank' class='inf' onclick='OpenWin(\"" + domain + ext + "\",this); return false;' href='javascript:;'> Xem thông tin</a>";
                                    $(cb_result).parent().html(new_object);
                                }
                                else {
                                    $(cb_result).show();
                                    $(cb_result).parent().find('img').remove();
                                }
                            },
                            failure: function (msg) {
                                console.log(msg);
                            }
                        });
                    }
                    else {
                        $(cb_result).parent().hide();
                    }
                }
                else {
                    $(cb_result).parent().hide();
                }
            });
        }
    }
    else {
        alert('Vui lòng nhập tên miền để kiểm tra!');
        $(".check-domain input[type='text']").focus();
    }
    return false;
}

$(function () {
    $('#whoisTrigger').fancybox({
        width: '680px',
        height: '540px',
        autoSize: false,
        fitToView: false,
        openEffect: 'fade',
        closeEffect: 'fade',
        'beforeShow': function () {
            $('#whois').html('');
        },
        'afterShow': function () { },
        'afterClose': function () { }
    });

    $("input[type='text'][name='tb_domain']").keypress(function (e) {
        if (e.keyCode == 13)
        {
            return checkDomain();
        }
    });

    $('.check-domain label, .check-domain-result label').hide();
    $('.check-domain label.common, .check-domain-result label.common').show();

    $('.domain-tabs a').click(function () {
        $('.domain-tabs a').removeAttr('class');
        $(this).attr('class', 'current');

        var type = $(this).attr('type');
        $('.check-domain label, .check-domain-result label').hide();
        $('.check-domain label.' + type + ', .check-domain-result label.' + type).show();
    });

    var temp = "";
    $(".check-domain input[type='checkbox']").each(function () {
        var labelclass = $(this).parent().attr('class');
        temp = temp +
            '<label class="' + labelclass + '">' +
                '<input type="checkbox" name="cb_ext_result_domain" value="' + $(this).val() + '" />' +
                '<span>' + $(this).val() + '</span>' +
            '</label>';
    });
    temp = temp + '<br class="clear" />';
    $("#res_container").html(temp);
});