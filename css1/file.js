let b1 = document.getElementById("on");
let b2 = document.getElementById("off");
let view = document.getElementById("view");

b1.addEventListener("click", function () {
  view.style.display = "block";
});

b2.addEventListener("click", function () {
  view.style.display = "none";
});

let b3 = document.getElementById("on1");
let view1 = document.getElementById("view1");

b3.addEventListener("click", function () {
  if (view1.style.display === "none" || view1.style.display === "") {
    view1.style.display = "block";
  } else {
    view1.style.display = "none";
  }
});

const currentPath = window.location.pathname;
const navLinks = document.querySelectorAll(".navigate nav ul li a");
navLinks.forEach(link => {
  const linkPath = new URL(link.href).pathname;
  if (linkPath === currentPath) {
    link.classList.add("active");
  }
});

document.querySelectorAll('.faqhead').forEach(head => {
  head.addEventListener('click', () => {
    const detail = head.nextElementSibling;
    const plusIcon = head.querySelector('.fa-angle-down');
    const minusIcon = head.querySelector('.fa-angle-up');
    detail.classList.toggle('show');
    if (detail.classList.contains('show')) {
      plusIcon.style.display = 'none';
      minusIcon.style.display = 'inline';
    } else {
      plusIcon.style.display = 'inline';
      minusIcon.style.display = 'none'; 
    }
  });
});

let fltbutton = document.getElementById("fltbutton");
let closebtn = document.getElementById("closebtn");
let fltcol = document.getElementById("fltcol");

fltbutton.addEventListener("click", function () {
  fltcol.style.display = "block";
});

closebtn.addEventListener("click", function () {
  fltcol.style.display = "none";
});