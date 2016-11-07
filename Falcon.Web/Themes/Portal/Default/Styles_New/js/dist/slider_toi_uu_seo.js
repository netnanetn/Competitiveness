var topContent = $('#content-page').offset().top;
var heightWindow = $(window).height();
var heightImg = $('#box-img img').height();
var topPosition = (heightWindow - heightImg) / 2;
var heightContent = $('#content-page').height();
// var topPosition = 95;

function resizePage() {
    var widthDiv = $('.content-page-left').width();
    $('#box-img').css('width', widthDiv);
    $('#box-img').fadeIn(150);
    topContent = $('#content-page').offset().top;
    heightWindow = $(window).height();
    heightImg = $('#box-img img').height();
    topPosition = (heightWindow - heightImg) / 2;
    // topPosition = 95;
    heightContent = $('#content-page').height();
    $('#box-img ul').css('top', (heightImg - $('#box-img ul').height()) / 2);
}

$('#box-img').css('margin-top', topPosition);
$(window).scroll(function () {
    if ($(window).scrollTop() > topContent) {
        if ($(window).scrollTop() > (topContent + heightContent - topPosition - heightWindow)) {
            $('#box-img').css('position', 'relative');
            $('#box-img').css('top', heightContent - heightWindow - 30);
            $('#box-img').css('margin-top', 0);
        } else {
            $('#box-img').css('position', 'fixed');
            $('#box-img').css('top', topPosition);
            $('#box-img').css('margin-top', 0);
        }
    } else {
        $('#box-img').css('position', 'relative');
        $('#box-img').css('top', 'initial');
        $('#box-img').css('margin-top', topPosition);
    }

    if ($(window).scrollTop() >= $('#page-item1').offset().top - 300 && $(window).scrollTop() < $('#page-item2').offset().top - 300) {
        $('.menu-img').removeClass('active-img');
        $('#menu-img1').addClass('active-img');
        if ($('#page-item1 .url-img').attr('src') != $('#box-img img').attr('src')) {

            $('#box-img img').hide();
            $('#box-img img').attr('src', $('#page-item1 .url-img').attr('src'));
            $('#box-img img').fadeIn(300);
        }
    }

    if ($(window).scrollTop() >= $('#page-item2').offset().top - 300 && $(window).scrollTop() < $('#page-item3').offset().top - 300) {
        $('.menu-img').removeClass('active-img');
        $('#menu-img2').addClass('active-img');
        if ($('#page-item2 .url-img').attr('src') != $('#box-img img').attr('src')) {

            $('#box-img img').hide();
            $('#box-img img').attr('src', $('#page-item2 .url-img').attr('src'));
            $('#box-img img').fadeIn(300);
        }
    }

    if ($(window).scrollTop() >= $('#page-item3').offset().top - 300) {
        $('.menu-img').removeClass('active-img');
        $('#menu-img3').addClass('active-img');
        if ($('#page-item3 .url-img').attr('src') != $('#box-img img').attr('src')) {
            $('#box-img img').hide();
            $('#box-img img').attr('src', $('#page-item3 .url-img').attr('src'));
            $('#box-img img').fadeIn(300);
        }
    }
})

resizePage();
$(window).resize(function () {
    resizePage();
});

$(".link-nav1").click(function () {
    $('html, body').animate({
        scrollTop: $(".intro-sieuweb").offset().top
    }, 500);
});