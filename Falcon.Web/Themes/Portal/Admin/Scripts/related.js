$(function () {
    $(".color-container input[type='text']").ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            $(el).val(hex);
            $(el).ColorPickerHide();
        }
    }).bind("keyup", function () {
        $(this).ColorPickerSetColor(this.value);
    });
});

function AppendSkinColor() {
    var count = $("#skincolor").find("div").length;
    var rowHTML =
        "<div>" +
            "<input type=\"text\" class=\"input-text\" name=\"Color_-" + count + "\" maxlength=\"6\" />" +
            "<select name=\"EcomList_-" + count + "\">" + $("#opt").val() + "</select>" +
            "<a href=\"javascript:;\" class=\"append\" onclick=\"AppendSkinColor();\">" +
                "<img src=\"/Themes/Portal/Admin/Images/addicon.png\" alt=\"\" /></a>" +
            "<a href=\"javascript:;\" class=\"remove\" onclick=\"RemoveSkinColor(this);\">" +
                "<img src=\"/Themes/Portal/Admin/Images/delicon.png\" alt=\"\" /></a>" +
        "</div>";
    $("#skincolor").append(rowHTML);
    $("input[name='Color_-" + count + "']").ColorPicker({
        onSubmit: function (hsb, hex, rgb, el) {
            $(el).val(hex);
            $(el).ColorPickerHide();
        }
    }).bind("keyup", function () {
        $(this).ColorPickerSetColor(this.value);
    });
}

function RemoveSkinColor(obj) {
    $(obj).parent().fadeTo("fast", 0, function () {
        $(this).slideUp("fast", function () {
            $(this).remove();

            var count = $("#skincolor").find("div").length;
            if (count <= 0) {
                AppendSkinColor();
            }
        });
    });
}