/*!
* Falcon Framework version 1.0
* http://hangtot.com/
*
* This script's content some common functions
* Copyright 2012, DKT Technology
*
* Date: 2012/08/24
*/

/*BEGIN Startup function*/
$(function () {
    $("img.lazyload").lazyload({
        placeholder: "/Themes/Portal/Default/Images/bg_lazyload_140.gif",
        effect: "fadeIn"
    });

    $('textarea.autoresize').autosize();  
});
/*END Startup function*/

String.prototype.trim = function () { return this.replace(/^\s\s*/, '').replace(/\s\s*$/, ''); };
String.prototype.ltrim = function () { return this.replace(/^\s+/, ''); }
String.prototype.rtrim = function () { return this.replace(/\s+$/, ''); }
String.prototype.fulltrim = function () { return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' '); }

function isNumeric(e) {
    var key = (window.event) ? window.event.keyCode : e.charCode;
    var nkey = String.fromCharCode(key);
    if (key != 0 && nkey.match(/[^0-9]/)) {
        KeypressThrow(e);
    }
}
function KeypressThrow(e) {
    if (window.event)
        window.event.returnValue = null;
    else
        if (e.preventDefault)
            e.preventDefault();
    e.returnValue = false;
    return false;
}
function isUrl(s) {
    var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@@!\-\/]))?/
    return regexp.test(s);
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : ";path=/; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function changeCity(cId) {
    setCookie("hangtot_City", cId, 365);
    window.location.reload();
}

/*BEGIN Product Details*/
function displayYM(name, im, msg) {
    document.write('<a title="' + name + '" style="text-decoration:none" href="ymsgr:sendim?' + im + '&amp;m=' + msg + '"><img alt="Yahoo" src="http://opi.yahoo.com/online?u=' + im + '&amp;m=g&amp;t=1&amp;l=us"></a>');
}

function displaySkype(name, nick) {
    document.write('<a title="' + name + '" href="skype:' + nick + '?chat" style="text-decoration:none"><img src="http://mystatus.skype.com/smallclassic/' + nick + '" style="border: none;" width="114" height="20" alt="Skype" /></a>');
}
/*END Product Details*/

/*Base64 encode, decode from stringencoders.googlecode.com*/
base64 = {}; base64.PADCHAR = "="; base64.ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"; base64.getbyte64 = function (a, b) { var c = base64.ALPHA.indexOf(a.charAt(b)); if (c == -1) { throw "Cannot decode base64" } return c }; base64.decode = function (a) { a = "" + a; var b = base64.getbyte64; var c, d, e; var f = a.length; if (f == 0) { return a } if (f % 4 != 0) { throw "Cannot decode base64" } c = 0; if (a.charAt(f - 1) == base64.PADCHAR) { c = 1; if (a.charAt(f - 2) == base64.PADCHAR) { c = 2 } f -= 4 } var g = []; for (d = 0; d < f; d += 4) { e = b(a, d) << 18 | b(a, d + 1) << 12 | b(a, d + 2) << 6 | b(a, d + 3); g.push(String.fromCharCode(e >> 16, e >> 8 & 255, e & 255)) } switch (c) { case 1: e = b(a, d) << 18 | b(a, d + 1) << 12 | b(a, d + 2) << 6; g.push(String.fromCharCode(e >> 16, e >> 8 & 255)); break; case 2: e = b(a, d) << 18 | b(a, d + 1) << 12; g.push(String.fromCharCode(e >> 16)); break } return g.join("") }; base64.getbyte = function (a, b) { var c = a.charCodeAt(b); if (c > 255) { throw "INVALID_CHARACTER_ERR: DOM Exception 5" } return c }; base64.encode = function (a) { if (arguments.length != 1) { throw "SyntaxError: Not enough arguments" } var b = base64.PADCHAR; var c = base64.ALPHA; var d = base64.getbyte; var e, f; var g = []; a = "" + a; var h = a.length - a.length % 3; if (a.length == 0) { return a } for (e = 0; e < h; e += 3) { f = d(a, e) << 16 | d(a, e + 1) << 8 | d(a, e + 2); g.push(c.charAt(f >> 18)); g.push(c.charAt(f >> 12 & 63)); g.push(c.charAt(f >> 6 & 63)); g.push(c.charAt(f & 63)) } switch (a.length - h) { case 1: f = d(a, e) << 16; g.push(c.charAt(f >> 18) + c.charAt(f >> 12 & 63) + b + b); break; case 2: f = d(a, e) << 16 | d(a, e + 1) << 8; g.push(c.charAt(f >> 18) + c.charAt(f >> 12 & 63) + c.charAt(f >> 6 & 63) + b); break } return g.join("") }