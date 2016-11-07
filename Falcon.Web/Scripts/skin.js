function changeSkinCategory(obj) {
    $(".category-item").removeClass("actived");
    $(".skins-by-category").hide();

    var categoryid = $(obj).attr("categoryid");
    $("#skincategory_" + categoryid).show();
    $(obj).parent().addClass("actived");
    return false;
}
$(function () {
    $($(".skins-by-category")[0]).show();
    $(".zoom").fancybox({
        openEffect: 'elastic',
        closeEffect: 'elastic'
    });
    $(".related-skin input[type='radio']").click(function () {
        var container = $(this).parent().parent();
        $(container).find("label").removeClass("selected");
        $(this).parent().addClass("selected");
    });
});