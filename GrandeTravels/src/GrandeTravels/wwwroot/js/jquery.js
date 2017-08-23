
$(document).ready(function () {

    //nav-overlay(open)
    $('closeBtn').click(function () {
        ('.overlay')
    });

    //welcome animation
    $(window).scroll(function () {
        if ($(document).scrollTop() > 200) {
            $('index-fireplace').animate({ left: '250px' });
        }
    });
});


