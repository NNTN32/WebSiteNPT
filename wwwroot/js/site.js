// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let mybutton = document.getElementById("myBtn");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

//$(document).ready(function){
//    $('body').on('click', '.btnAddToCart', function (e)){
        
//        $.ajax({
//            url: '/ShoppingCart/AddToCart',
//            type: 'POST',
//            data: { id: ProductID, quantity: Quantity },
//            success: function (rs) {
//                if (rs.success) {
//                    $('#checkout_items').html(rs.Count);
//                    alert(rs.msg);
//                }
//            }
//        });
//    });
//});