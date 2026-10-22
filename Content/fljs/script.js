(function($) {
"use strict";
	$(document).ready(function () {
		
		$( ".countryTab" ).tabs();
		
		// Modify slid show
		$("#changeflights").click(function () {
			debugger;
			$("#changeflight1").slideToggle();
		
		if ($("#changeflights i")) {
			$("#changeflights").html("<i class='fa-solid fa-pen-to-square'></i>");
		}
		else {
			$("#changeflights").html("<i class='fa-solid fa-pen-to-square'></i>");
		}
		});
	
		$('#top-slider').owlCarousel({
		nav:true,
		items:1,
		dots: true,
		navText: false,
		responsive:{
			0:{ items:1 },
			600:{ items:1 },
			1000:{ items:1 }
		}
	});
	$(".owl-prev").addClass("fa fa-angle-left");
	$(".owl-next").addClass("fa fa-angle-right");
	$('#countries-slider').owlCarousel({
		nav:false,
		items:6,
		dots: true,
		navText: false,
		margin: 20,
		responsive:false,
		responsive:{
			0:{ items:1 },
			600:{ items:2 },
			980:{ items:3 }
		}
	});
	$('#destination-slider').owlCarousel({
		nav:true,
		items:1,
		dots: false,
		navText: false,
		responsive:{
			0:{ items:1 },
			600:{ items:1 },
			1000:{ items:1 }
		}
	});
	$("#destination-slider .owl-prev").html("<i class='fa fa-angle-left'></i>");
	$("#destination-slider .owl-next").html("<i class='fa fa-angle-right'></i>");
	
	
$('#main-slider').owlCarousel({ nav:!0,items:3,dots:!1,margin:10,navText:!1,loop:!0,responsive:{0:{items:1},550:{items:2},1e3:{items:3}}});
$("#main-slider .owl-prev").addClass("lftarrow fa fa-long-arrow-left");
$("#main-slider .owl-next").addClass("rgtarrow fa fa-long-arrow-right");

$('#get-slider').owlCarousel({ nav:!0,items:3,dots:!1,margin:10,navText:!1,loop:!0,responsive:{0:{items:1},550:{items:2},1e3:{items:3}}});
$("#get-slider .owl-prev").addClass("lftarrow fa fa-long-arrow-left");
$("#get-slider .owl-next").addClass("rgtarrow fa fa-long-arrow-right");	
		
});

		

})(jQuery);