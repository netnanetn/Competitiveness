$(function () {
    function FloatTopDiv() {
        startLX = ((document.body.clientWidth - MainContentW) / 2) - LeftBannerW - LeftAdjust, startLY = TopAdjust + 80;
        startRX = ((document.body.clientWidth - MainContentW) / 2) + MainContentW + RightAdjust, startRY = TopAdjust + 80;
        var d = document;
        function ml(id) {
            var el = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
            el.sP = function (x, y) { this.style.left = x + 'px'; this.style.top = y + 'px'; };
            el.x = startRX;
            el.y = startRY;
            return el;
        }
        function m2(id) {
            var e2 = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
            e2.sP = function (x, y) { this.style.left = x + 'px'; this.style.top = y + 'px'; };
            e2.x = startLX;
            e2.y = startLY;
            return e2;
        }
        window.stayTopLeft = function () {
            if (document.documentElement && document.documentElement.scrollTop)
                var pY = document.documentElement.scrollTop;
            else if (document.body)
                var pY = document.body.scrollTop;
            if (document.body.scrollTop > 30) { startLY = 3; startRY = 3; }
            else { startLY = TopAdjust; startRY = TopAdjust; };
            ftlObj.y += (pY + startRY - ftlObj.y) / 16;
            ftlObj.sP(ftlObj.x, ftlObj.y);
            ftlObj2.y += (pY + startLY - ftlObj2.y) / 16;
            ftlObj2.sP(ftlObj2.x, ftlObj2.y);
            setTimeout("stayTopLeft()", 1);
        }
        ftlObj = ml("divAdRight");
        ftlObj2 = m2("divAdLeft");
        stayTopLeft();
    }
    function ShowAdDiv() {
        var objAdDivRight = document.getElementById("divAdRight");
        var objAdDivLeft = document.getElementById("divAdLeft");
        if (document.body.clientWidth < parseInt(MainContentW) + parseInt(128 * 2 + 2)) {
            objAdDivRight.style.display = "none";
            objAdDivLeft.style.display = "none";
        }
        else {
            objAdDivRight.style.display = "block";
            objAdDivLeft.style.display = "block";
            FloatTopDiv();
        }
    }
    var containerWidth = 1000;
    var windowWidth = $(window).width();

    MainContentW = containerWidth + 30; // chieu rong khung noi dung
    LeftBannerW = 276; // chieu rong quang cao ben trai
    RightBannerW = 276; // chieu rong quang cao ben phai
    LeftAdjust = 0;
    RightAdjust = -130;
    TopAdjust = 550;
    ShowAdDiv();
    window.onresize = ShowAdDiv;
});

$(function () {
    $(".counselor .show").click(function () {
        var expand = $("#expand").css("display");
        if (expand == "" || expand == "block") {
            TopAdjust = 100;
            $('#persons').slideDown('fast');
            $('#collapse').show();
            $("#expand").hide();
        }
        else {
            TopAdjust = 550;
            $('#persons').slideUp('fast');
            $('#expand').show();
            $("#collapse").hide();
        }
        return false;
    });
});