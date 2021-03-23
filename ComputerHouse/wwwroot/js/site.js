// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('#back-to-top').fadeIn();
        } else {
            $('#back-to-top').fadeOut();
        }
    });
    // scroll body to 0px on click
    $('#back-to-top').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 600);
        return false;
    });

    //var scroll = $(window).scrollTop();
    //alert(scroll);

    //Get height of top-header, main-header and main-footer and add css min-height=100%-(main-header+top-header+main-footer);
    //To adjust the footer at the bottom of the browser when the content is empty.
    var heightHeaderTop = $('.header-top').outerHeight();
    var heightMainTop = $('.header-main').outerHeight();

    var headerHeightWithBorder = heightHeaderTop + heightMainTop;

    var heightOfWindow = window.innerHeight;

    var bodyContainerHeight = heightOfWindow - headerHeightWithBorder;

    $(".body-container").css("min-height", bodyContainerHeight);
    //alert(heightMainTop);

    //Shopping Cart Popover
    //$("#js-shopping-cart").popover({
    //    title: "Shopping Cart",
    //    htm: true,
    //    placement: "bottom",
    //    trigger: 'focus',
    //    content: function () {
    //        return "Hello."
    //    }
    //});

    $("#js-shopping-cart").popover({
        title:`<h5 class="text-primary text-center"><b>My Shopping Cart</b></h5>`,
        trigger: "focus",
        placement: "bottom",
        html: true,
        content: function () {
            return `<h6>You Have No Items In Cart.</h4><br/>
                <a href='/Customer/Shoppings/Index' class='btn btn-info form-control'>Start  Adding</a>`;
        }
    });

});