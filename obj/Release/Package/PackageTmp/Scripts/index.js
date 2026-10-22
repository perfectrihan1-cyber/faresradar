//https://images.unsplash.com/photo-1495822892661-2ead864e1c7b?auto=format&fit=crop&w=2689&q=80
//https://images.unsplash.com/photo-1490380169520-0a4b88d52565?auto=format&fit=crop&w=1950&q=80
//https://images.unsplash.com/photo-1499561385668-5ebdb06a79bc?auto=format&fit=crop&w=1949&q=80
//Using jQuery on my actual project, but this could've 
//been rewritten without it
var one_way_bool = true;


$(function () {

    $("#loader").hide()
    $(".one_way_btn").click(function () {
        $(".one_way_btn").css({
            "background-color": "black",
            "color": "white"
        });
        $(".round_trip_btn").css({
            "background-color": "transparent",
            "color": "black"
        });


        one_way_bool = true;

        $("#date-picker").css({
            "position": "absolute",
            "width": "200px",
            "color": "white",
            "left": "445px"
        });
        $("#from_auto").css({
            "width": "200px",
            "color": "white",
            "left": "0px"
        });
        $("#to_auto").css({
            "width": "200px",
            "color": "white",
            "left": "235px"
        });

        $("#search_btn").css({
            "left": "670px"
        });




        $(".datepicker-input").show()
        $(".daterangepicker-input").hide()
        $(".datepicker-input").empty()
        $(".daterangepicker-input").empty()
        $(".datepicker-input").each(function (i) {
            flatpickr("#" + this.id, {
                allowInput: true,
                enableTime: false,
                dateFormat: "y-m-d"
            });

        });

    });
    $(".round_trip_btn").click(function () {
        one_way_bool = false;
        $(".round_trip_btn").css({
            "background-color": "black",
            "color": "white"
        });

        //adjust positions of input and button tag
        $("#range-picker").css({
            "position": "absolute",
            "width": "390px",
            "color": "white",
            "left": "360px"
        });
        $("#from_auto").css({
            "width": "150px",
            "color": "white",
            "left": "0px"
        });
        $("#to_auto").css({
            "width": "180px",
            "color": "white",
            "left": "170px"
        });

        $("#search_btn").css({
            "left": "760px"
        });




        $(".one_way_btn").css({
            "background-color": "transparent",
            "color": "black"
        });
        $(".daterangepicker-input").attr("placeholder", "Date")
        $(".datepicker-input ").hide()
        $(".daterangepicker-input").show()
        $(".daterangepicker-input").empty()
        $(".datepicker-input").empty()
        $(".daterangepicker-input").each(function (i) {
            flatpickr("#" + this.id, {
                allowInput: true, //On or off doesn't change error
                enableTime: false,
                dateFormat: "y-m-d",
                mode: "range"
            });


        });

    });

});



var options = {
    shouldSort: true,
    threshold: 0.4,
    maxPatternLength: 32,
    keys: [{
        name: 'iata',
        weight: 0.5
    }, {
        name: 'name',
        weight: 0.3
    }, {
        name: 'city',
        weight: 0.2
    }]
};

var fuse = new Fuse(airports, options)


var ac = $('.autocomplete_from')
    .on('click', function (e) {
        e.stopPropagation();
    })
    .on('focus keyup', search)
    .on('keydown', onKeyDown);

var wrap = $('<div>')
    .addClass('autocomplete-wrapper')
    .insertBefore(ac)
    .append(ac);

var list = $('<div>')
    .addClass('autocomplete-results')
    .on('click', '.autocomplete-result', function (e) {
        e.preventDefault();
        e.stopPropagation();
        selectIndex($(this).data('index'));
        //ankit
    })
    .appendTo(wrap);

$(document)
    .on('mouseover', '.autocomplete-result', function (e) {
        var index = parseInt($(this).data('index'), 10);
        if (!isNaN(index)) {
            list.attr('data-highlight', index);
        }
    })
    .on('click', clearResults);

function clearResults() {
    results = [];
    numResults = 0;
    list.empty();
}

function selectIndex(index) {
    if (results.length >= index + 1) {
        ac.val(results[index].iata + "-" + results[index].name + " " + results[index].city);
        clearResults();
    }
}

var results = [];
var numResults = 0;
var selectedIndex = -1;

function search(e) {
    if (e.which === 38 || e.which === 13 || e.which === 40) {
        return;
    }

    if (ac.val().length > 0) {
        results = _.take(fuse.search(ac.val()), 7);
        numResults = results.length;

        var divs = results.map(function (r, i) {
            return '<div class="autocomplete-result" data-index="' + i + '">' +
                '<div><b>' + r.iata + '</b> - ' + r.name + '</div>' +
                '<div class="autocomplete-location">' + r.city + ', ' + r.country + '</div>' +
                '</div>';
        });

        selectedIndex = -1;
        list.html(divs.join(''))
            .attr('data-highlight', selectedIndex);

    } else {
        numResults = 0;
        list.empty();
    }
}

function onKeyDown(e) {
    switch (e.which) {
        case 38: // up
            selectedIndex--;
            if (selectedIndex <= -1) {
                selectedIndex = -1;
            }
            list.attr('data-highlight', selectedIndex);
            break;
        case 13: // enter
            selectIndex(selectedIndex);
            break;
        case 9: // enter
            selectIndex(selectedIndex);
            e.stopPropagation();
            return;
        case 40: // down
            selectedIndex++;
            if (selectedIndex >= numResults) {
                selectedIndex = numResults - 1;
            }
            list.attr('data-highlight', selectedIndex);
            break;

        default:
            return; // exit this handler for other keys
    }
    e.stopPropagation();
    e.preventDefault(); // prevent the default action (scroll / move caret)
}


var ac_to = $('.autocomplete_to') //here
    .on('click', function (e) {
        e.stopPropagation();
    })
    .on('focus keyup', search_to)
    .on('keydown', onKeyDown_to);

var wrap_to = $('<div>')
    .addClass('autocomplete-wrapper')
    .insertBefore(ac_to)
    .append(ac_to);

var list_to = $('<div>')
    .addClass('autocomplete-results_to')
    .on('click', '.autocomplete-result', function (e) {
        e.preventDefault();
        e.stopPropagation();
        selectIndex_to($(this).data('index'));
    })
    .appendTo(wrap_to);

$(document)
    .on('mouseover', '.autocomplete-result', function (e) {
        var index = parseInt($(this).data('index'), 10);
        if (!isNaN(index)) {
            list_to.attr('data-highlight', index);
        }
    })
    .on('click', clearResults_to);

function clearResults_to() {
    results = [];
    numResults = 0;
    list_to.empty();
}

function selectIndex_to(index) { //here
    if (results.length >= index + 1) {
        ac_to.val(results[index].iata + "-" + results[index].name + " " + results[index].city);
        clearResults_to();
    }
}

var results = [];
var numResults = 0;
var selectedIndex = -1;

function search_to(e) {
    if (e.which === 38 || e.which === 13 || e.which === 40) {
        return;
    }

    if (ac_to.val().length > 0) {
        results = _.take(fuse.search(ac_to.val()), 7);
        numResults = results.length;

        var divs = results.map(function (r, i) {
            return '<div class="autocomplete-result" data-index="' + i + '">' +
                '<div><b>' + r.iata + '</b> - ' + r.name + '</div>' +
                '<div class="autocomplete-location">' + r.city + ', ' + r.country + '</div>' +
                '</div>';
        });

        selectedIndex = -1;
        list_to.html(divs.join(''))
            .attr('data-highlight', selectedIndex);

    } else {
        numResults = 0;
        list_to.empty();
    }
}

function onKeyDown_to(e) {
    switch (e.which) {
        case 38: // up
            selectedIndex--;
            if (selectedIndex <= -1) {
                selectedIndex = -1;
            }
            list_to.attr('data-highlight', selectedIndex_to);
            break;
        case 13: // enter
            selectIndex_to(selectedIndex);
            break;
        case 9: // enter
            selectIndex_to(selectedIndex);
            e.stopPropagation();
            return;
        case 40: // down
            selectedIndex++;
            if (selectedIndex >= numResults) {
                selectedIndex = numResults - 1;
            }
            list_to.attr('data-highlight', selectIndex_to);
            break;

        default:
            return; // exit this handler for other keys
    }
    e.stopPropagation();
    e.preventDefault(); // prevent the default action (scroll / move caret)
}
