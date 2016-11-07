/// <copyright file="admin.js" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description>Contain functions use for admin</description>

function setLocation(url) {    
    window.location.href = url;
}

/**
    Confirm before delete data, using post method for security
**/
function deleteConfirm(confirmMsg, postUrl) {
    if (confirm(confirmMsg)) {
        $.post(postUrl, function (data) {
            //reload after delete            
            window.location.href = window.location.href;
        });
    }
    return false;
}
