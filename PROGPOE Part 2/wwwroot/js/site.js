// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function swapLoginText() {
    //event.preventDefault();
    let labelElement = document.getElementById("Login");

    if (true) {
        labelElement.innerHTML = "<em>New Text</em> using <strong>innerHTML</strong>";
    }
    
}

function showHideFilters() {
    var btnFilter = document.getElementById("btnFilter");
    
    btnFilter.addEventListener("click", function () {
        var content = document.getElementById("Filters");
        content.classList.toggle("show");

        if (content.classList.contains("show")) {
            btnFilter.innerHTML = "∧"; // Change to up arrow
        } else {
            btnFilter.innerHTML = "∨"; // Change to down arrow
        }
    });
}

function toHome() {
    window.location.href = '/Home/Index';
}

function logOut() {
    window.location.href = '/Account/Logout';
}

function addNew() {
    window.location.href = '/Farmer/Create';
}

document.addEventListener("DOMContentLoaded", function () {
    showHideFilters();
});


//Carousel Scripts

let slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}