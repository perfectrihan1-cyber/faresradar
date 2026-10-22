setInterval(mufun, 1000);
function mufun() {
  let b1 = document.getElementById("on");
  let b2 = document.getElementById("off");
  let view = document.getElementById("view");

  b1.addEventListener("click", function () {
    if ((view.style.display = "none")) {
      view.style.display = "block";

    }
  })

  b2.addEventListener("click", function () {
    if ((view.style.display = "block")) {
      view.style.display = "none";
    }
  })
}

window.addEventListener('scroll', () => {
  const header = document.querySelector('.newheader');
  if (window.scrollY > 100) { // Adjust the threshold as needed
    header.classList.add('scrolled');
  } else {
    header.classList.remove('scrolled');
  }
});

const columnContainer = document.querySelector('.showhide');
const columns = columnContainer.querySelectorAll('div');
const images = [
    document.getElementById('img1'),
    document.getElementById('img2'),
    document.getElementById('img3'),
    document.getElementById('img4')
];

function hideAllImages() {
    images.forEach(img => img.style.display = 'none');
}

function showImage(imageIndex) {
    hideAllImages();
    images[imageIndex].style.display = 'block';
}

window.addEventListener('load', () => {
    showImage(0);
});

let activeColumnIndex = 0; // Keep track of the active column

columns.forEach((col, index) => {
    col.addEventListener('mouseenter', () => {
        showImage(index);

        // Set active column index
        activeColumnIndex = index;

         // Add border to the hovered column *and* image container
        col.style.borderColor = '#1dadee';
        images[index].parentElement.style.borderColor = '#1dadee'; // Border to parent of image

        // Remove border from other columns and their associated images
        columns.forEach((otherCol, otherIndex) => {
            if (otherIndex !== index) {
                otherCol.style.borderColor = ''; // Reset column border
                images[otherIndex].parentElement.style.borderColor = ''; //Reset image container border
            }
        });
    });

    // Remove mouseleave event listener completely
});

// Set initial border on load
columns[activeColumnIndex].style.borderColor = '#1dadee';
images[activeColumnIndex].parentElement.style.borderColor = '#1dadee';