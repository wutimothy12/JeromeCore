$('#responsive-menu-button').sidr({
    name: 'sidr-main',
    source: '#nav'
});

$('body').swipe({
    //Single swipe handler for left swipes
    swipeLeft: function () {
        $.sidr('close', 'sidr-main');
        jQuery("#responsive-menu-button").removeClass("active");
    },
    swipeRight: function () {
        $.sidr('open', 'sidr-main');
        jQuery("#responsive-menu-button").addClass("active");
    },
    //Default is 75px, set to 0 for demo so any distance triggers swipe
    threshold: 45,
    preventDefaultEvents: false
});


jQuery("#responsive-menu-button").on("click", function () {
    jQuery(this).toggleClass("active");
});
jQuery("#responsive-menu-button").on("touchstart", function () {
    jQuery(this).toggleClass("active");
});


$(document).ready(function () {
    var offset = 100;
    var duration = 500;
    jQuery(window).scroll(function () {
        if (jQuery(this).scrollTop() > offset) {
            jQuery('.scroll-to-top').fadeIn(duration);
        } else {
            jQuery('.scroll-to-top').fadeOut(duration);
        }
    });

    $('.scroll-to-top').click(function (event) {
        event.preventDefault();
        jQuery('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    })
});
