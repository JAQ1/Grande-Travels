
$(document).ready(function () {

    $('.btnDeleteCartPack').hide();

    //nav animation
    $('.navToggleBtn').click(function () {
        $('.navToggleBtn').toggleClass('active');

    });
    $('.navToggleBtn').click(function () {
        $('.overlay').toggleClass('open')
    });

    //home title fade in
    //$(window).scroll(function () {
    //    if ($(document).scrollTop() > 1) {
    //        $('#index-title').fadeOut();
    //    }
    //});
})