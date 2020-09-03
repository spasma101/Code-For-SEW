$(document).ready(function () {
    
    searchIconImageSwap();
    searchIconImageSwapMobile();
    hideShowMobileDesktopCategories();

    $(window).bind('resizeEnd', function() {
        //do something, window hasn't changed size in 300ms
        hideShowMobileDesktopCategories();
    });

});// END DOCUMENT.Ready

//Functions for blog
function searchIconImageSwap() {
    $('#blogSearch a').hover(mouseEnter, mouseLeave);
    function mouseEnter() {
        $('#blogSearch a img').attr('src', '/media/duefq4qf/search-pink.svg');
    };
    function mouseLeave() {
        $('#blogSearch a img').attr('src', '/media/frphzgrw/search-blue.svg');
    };
}
function searchIconImageSwapMobile() {
    $('#blogSearchMobile a').hover(mouseEnter, mouseLeave);
    function mouseEnter() {
        $('#blogSearchMobile a img').attr('src', '/media/duefq4qf/search-pink.svg');
    };
    function mouseLeave() {
        $('#blogSearchMobile a img').attr('src', '/media/frphzgrw/search-blue.svg');
    };
}

function hideShowMobileDesktopCategories() {
    if ($(window).width() <= 843) {
        $('#blogSearchMobile').css("display", "block");
        $('.Category__List').css("display", "none");
    } else {
        $('#blogSearchMobile').css("display", "none");
        $('.Category__List').css("display", "flex");

    }
}
//Utility Functions for Blog
$(window).resize(function() {
    if(this.resizeTO) clearTimeout(this.resizeTO);
    this.resizeTO = setTimeout(function() {
        $(this).trigger('resizeEnd');
    }, 300);
});