//document.addEventListener('DOMContentLoaded', function () {
//    let carouselItems = document.querySelectorAll('.carousel-item');
//    let currentIndex = 0;
//    let totalItems = carouselItems.length;

//    function moveToNextSlide() {
//        carouselItems[currentIndex].classList.remove('active');
//        currentIndex = (currentIndex + 1) % totalItems;
//        carouselItems[currentIndex].classList.add('active');
//    }

//    let carouselInterval = setInterval(moveToNextSlide, 3000);

//    document.querySelector('.carousel-control-next').addEventListener('click', function () {
//        clearInterval(carouselInterval);
//        moveToNextSlide();
//        carouselInterval = setInterval(moveToNextSlide, 3000);
//    });

//    document.querySelector('.carousel-control-prev').addEventListener('click', function () {
//        clearInterval(carouselInterval);
//        carouselItems[currentIndex].classList.remove('active');
//        currentIndex = (currentIndex - 1 + totalItems) % totalItems;
//        carouselItems[currentIndex].classList.add('active');
//        carouselInterval = setInterval(moveToNextSlide, 3000);
//    });
//});