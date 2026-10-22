function initializeSlider(sliderSelector, slideSelector, nextBtnSelector, prevBtnSelector, slidetoShow, gap = 0) {
    const slider = document.querySelector(sliderSelector);
    const slides = document.querySelectorAll(slideSelector);
    let currentIndex = 0;
    const totalSlides = slides.length;
    const slideWidth = slides[0].clientWidth;
  
    document.querySelector(nextBtnSelector).addEventListener('click', () => {
        moveToNextSlide();
    });
  
    document.querySelector(prevBtnSelector).addEventListener('click', () => {
        moveToPrevSlide();
    });
  
    function moveToNextSlide() {
        if (currentIndex < totalSlides - slidetoShow) {
            currentIndex++;
            updateSliderPosition();
        }
    }
  
    function moveToPrevSlide() {
        if (currentIndex > 0) {
            currentIndex--;
            updateSliderPosition();
        }
    }
  
    function updateSliderPosition() {
        const newPosition = -(slideWidth + gap) * currentIndex;
        slider.style.transform = `translateX(${newPosition}px)`;
    }
  
    // Mouse wheel functionality
    slider.addEventListener('wheel', (event) => {
        if (event.deltaY > 0) {
            moveToNextSlide();
        } else {
            moveToPrevSlide();
        }
    });
}
  
// Initialize the first slider
initializeSlider('.slider1', '.slide1', '.next-btn1', '.prev-btn1', 1, 10);
initializeSlider('.slider2', '.slide2', '.next-btn2', '.prev-btn2', 3, 15);
initializeSlider('.slider3', '.slide3', '.next-btn3', '.prev-btn3', 4, 15);