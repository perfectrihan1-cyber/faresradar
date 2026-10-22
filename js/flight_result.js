// Click on the collapsed search summary to open the full search form
document.addEventListener('DOMContentLoaded', function () {
  var collapsed = document.getElementById('skyCollapsed');
  var expanded = document.getElementById('skyExpanded');

  if (!collapsed || !expanded) return;

  collapsed.addEventListener('click', function () {
    collapsed.style.display = 'none';
    expanded.style.display = 'block';
  });

  // Click anywhere outside the search header to collapse the form back
  var skyHeader = document.getElementById('skyHeader');
  document.addEventListener('click', function (e) {
    if (expanded.style.display === 'block' && skyHeader && !skyHeader.contains(e.target)) {
      expanded.style.display = 'none';
      collapsed.style.display = '';
    }
  });

  // ── Filter checkboxes: check / uncheck ──
  document.querySelectorAll('.frow:not(.disabled)').forEach(function (row) {
    row.addEventListener('click', function () {
      var cb = row.querySelector('.fcb');
      var isOn = cb.classList.toggle('on');
      cb.innerHTML = isOn ? '&#10003;' : '';
    });
  });

  // ── Collapsible filter sections (Stops, Baggage, Airlines, Price, Times) ──
  document.querySelectorAll('.stitle .arr[data-sec]').forEach(function (arrow) {
    arrow.addEventListener('click', function () {
      var sec = arrow.dataset.sec;
      var body = document.getElementById('sec-' + sec);
      if (!body) return;
      arrow.classList.toggle('up');
      body.classList.toggle('collapsed');
    });
  });

  // ── Select all / Clear all (airlines) ──
  document.querySelectorAll('.fctrls').forEach(function (ctrls) {
    var container = ctrls.parentElement;
    var rows = container.querySelectorAll('.frow[data-filter="airline"]');
    var selectBtn = ctrls.children[0];
    var clearBtn = ctrls.children[1];

    if (selectBtn) {
      selectBtn.addEventListener('click', function () {
        rows.forEach(function (row) {
          var cb = row.querySelector('.fcb');
          cb.classList.add('on');
          cb.innerHTML = '&#10003;';
        });
      });
    }
    if (clearBtn) {
      clearBtn.addEventListener('click', function () {
        rows.forEach(function (row) {
          var cb = row.querySelector('.fcb');
          cb.classList.remove('on');
          cb.innerHTML = '';
        });
      });
    }
  });

  // ── Flight detail popup ──
  var LOGO_URLS = {
    'Air India Express': 'https://kimi-web-img.moonshot.cn/img/logos-world.net/e1b79ed3ed5ad384c9fbdc4a88919a1137c0ce81.png',
    'IndiGo': 'https://kimi-web-img.moonshot.cn/img/seekvectorlogo.com/daa2e905e356ca58b2aac3b575ec98cbf38bf518.png',
    'SpiceJet': 'https://kimi-web-img.moonshot.cn/img/logos-world.net/e60f2a503c7896496a511e4851cbae70fb72b954.png',
    'Akasa Air': 'https://kimi-web-img.moonshot.cn/img/upload.wikimedia.org/40e030e905b157ff66aed1393ee2e79b2476fe48.png'
  };

  var AIRPORT_NAMES = {
    DEL: 'Indira Gandhi Intl Airport',
    BOM: 'Chhatrapati Shivaji Maharaj Intl Airport'
  };

  // Flight data matching the 4 cards currently shown on the page
  var FLIGHTS = {
    1: { airline:'Air India Express', code:'AIX-101', price:270, from:'DEL', to:'BOM', dep:'16:15', arr:'18:20', dur:'2h 5m', bag:'none',
         retFrom:'BOM', retTo:'DEL', retDep:'21:55', retArr:'23:55', retDur:'2h 0m' },
    7: { airline:'Air India Express', code:'AIX-707', price:200, from:'DEL', to:'BOM', dep:'14:00', arr:'15:55', dur:'1h 55m', bag:'checked',
         retFrom:'BOM', retTo:'DEL', retDep:'17:00', retArr:'19:05', retDur:'2h 5m' },
    6: { airline:'IndiGo', code:'6E-606', price:250, from:'DEL', to:'BOM', dep:'05:30', arr:'07:30', dur:'2h 0m', bag:'none',
         retFrom:'BOM', retTo:'DEL', retDep:'22:00', retArr:'00:10', retDur:'2h 10m' },
    8: { airline:'SpiceJet', code:'SG-808', price:813, from:'DEL', to:'BOM', dep:'08:00', arr:'09:58', dur:'1h 58m', bag:'cabin',
         retFrom:'BOM', retTo:'DEL', retDep:'12:00', retArr:'14:05', retDur:'2h 5m' }
  };

  function bagLabel(bag) {
    if (bag === 'cabin') return '🧳 Cabin: 7 KG included';
    if (bag === 'checked') return '🧳 Cabin + 1 checked bag';
    return '🧳 No free baggage';
  }

  function legHTML(airline, code, dep, arr, from, to, bag) {
    var logo = LOGO_URLS[airline] || '';
    return (
      '<div class="fd-leg">' +
        '<div class="fd-leg-header">' +
          '<img class="fd-airline-logo" src="' + logo + '" alt="' + airline + '">' +
          '<span class="fd-airline-name">' + airline + '</span>' +
          '<span class="fd-flight-no">' + code + '</span>' +
        '</div>' +
        '<div class="fd-times">' +
          '<div class="fd-time-block">' +
            '<div class="fd-time">' + dep + '</div>' +
            '<div class="fd-airport-code">' + from + '</div>' +
            '<div class="fd-airport-name">' + (AIRPORT_NAMES[from] || '') + '</div>' +
          '</div>' +
          '<div class="fd-arrow-mid"><div class="fd-arrow-plane">✈</div><div class="fd-arrow-line"></div></div>' +
          '<div class="fd-time-block right">' +
            '<div class="fd-time">' + arr + '</div>' +
            '<div class="fd-airport-code">' + to + '</div>' +
            '<div class="fd-airport-name">' + (AIRPORT_NAMES[to] || '') + '</div>' +
          '</div>' +
        '</div>' +
        '<div class="fd-baggage"><span class="fd-baggage-icon"></span>' + bagLabel(bag) + '</div>' +
      '</div>'
    );
  }

  var fdOverlay = document.getElementById('fdOverlay');
  var fdModal = document.getElementById('fdModal');
  var fdTitle = document.getElementById('fdTitle');
  var fdBody = document.getElementById('fdBody');
  var fdPrice = document.getElementById('fdPrice');
  var fdClose = document.querySelector('.fd-close');

  function openPopup(flId) {
    var fl = FLIGHTS[flId];
    if (!fl) return;

    var outboundHTML =
      '<div class="fd-section">' +
        '<div class="fd-sec-title">Outbound</div>' +
        '<div class="fd-date-dur"><span>Tue 16 Jul 26</span><span>Duration: ' + fl.dur + '</span></div>' +
        legHTML(fl.airline, fl.code, fl.dep, fl.arr, fl.from, fl.to, fl.bag) +
      '</div>';

    var inboundHTML =
      '<div class="fd-section">' +
        '<div class="fd-sec-title">Inbound</div>' +
        '<div class="fd-date-dur"><span>Fri 24 Jul 26</span><span>Duration: ' + fl.retDur + '</span></div>' +
        legHTML(fl.airline, fl.code + 'R', fl.retDep, fl.retArr, fl.retFrom, fl.retTo, fl.bag) +
      '</div>';

    fdBody.innerHTML = outboundHTML + inboundHTML;
    fdTitle.textContent = fl.airline + ' – ' + fl.from + ' \u2194 ' + fl.to;
    fdPrice.textContent = '$' + fl.price;

    fdOverlay.classList.add('show');
    fdModal.classList.add('show');
    document.body.style.overflow = 'hidden';
  }

  function closePopup() {
    fdOverlay.classList.remove('show');
    fdModal.classList.remove('show');
    document.body.style.overflow = '';
  }

  document.querySelectorAll('.fcard').forEach(function (card) {
    card.addEventListener('click', function () {
      openPopup(card.dataset.id);
    });

    var btn = card.querySelector('.btn-sel');
    if (btn) {
      btn.addEventListener('click', function (e) {
        e.stopPropagation();
      });
    }

    var heart = card.querySelector('.heart-btn');
    if (heart) {
      heart.addEventListener('click', function (e) {
        e.stopPropagation();
        heart.textContent = heart.textContent === '♡' ? '❤' : '♡';
      });
    }
  });

  if (fdClose) fdClose.addEventListener('click', closePopup);
  if (fdOverlay) fdOverlay.addEventListener('click', closePopup);

  // ── Trip type dropdown (Return / One way / Multi-city) ──
  var tripTypeBtn = document.getElementById('tripTypeBtn');
  var tripTypeDropdown = document.getElementById('tripTypeDropdown');
  var tripTypeLabel = document.getElementById('tripTypeLabel');

  if (tripTypeBtn && tripTypeDropdown) {
    tripTypeBtn.addEventListener('click', function (e) {
      e.stopPropagation();
      tripTypeDropdown.classList.toggle('open');
    });

    tripTypeDropdown.querySelectorAll('.sky-type-opt').forEach(function (opt, idx) {
      opt.addEventListener('click', function (e) {
        e.stopPropagation();
        tripTypeLabel.textContent = opt.textContent;
        tripTypeDropdown.classList.remove('open');

        var returnField = document.getElementById('returnField');
        if (returnField) {
          // idx 0 = Return, idx 1 = One way, idx 2 = Multi-city
          returnField.style.display = (idx === 1) ? 'none' : '';
        }
      });
    });
  }

  // ── Traveller dropdown (Adults / Children / Infants / Cabin) ──
  var travellerBtn = document.getElementById('travellerBtn');
  var travellerDropdown = document.getElementById('travellerDropdown');
  var travellerLabel = document.getElementById('travellerLabel');
  var adultCountEl = document.getElementById('adultCount');
  var childCountEl = document.getElementById('childCount');
  var infantCountEl = document.getElementById('infantCount');
  var cabinSelect = document.getElementById('cabinSelect');
  var doneBtn = travellerDropdown ? travellerDropdown.querySelector('.sky-tr-done') : null;

  var counts = { adult: 1, child: 0, infant: 0 };

  function updateTravellerLabel() {
    var total = counts.adult + counts.child + counts.infant;
    var who = total + (total === 1 ? ' Adult' : ' Traveller' + (total === 1 ? '' : 's'));
    if (counts.child === 0 && counts.infant === 0) {
      who = counts.adult + (counts.adult === 1 ? ' Adult' : ' Adults');
    }
    travellerLabel.textContent = who + ', ' + (cabinSelect ? cabinSelect.value : 'Economy');
  }

  if (travellerBtn && travellerDropdown) {
    travellerBtn.addEventListener('click', function (e) {
      e.stopPropagation();
      travellerDropdown.classList.toggle('open');
    });

    travellerDropdown.addEventListener('click', function (e) {
      e.stopPropagation();
    });

    travellerDropdown.querySelectorAll('.sky-tr-row').forEach(function (row, idx) {
      var key = idx === 0 ? 'adult' : idx === 1 ? 'child' : 'infant';
      var countEl = idx === 0 ? adultCountEl : idx === 1 ? childCountEl : infantCountEl;
      var minus = row.querySelectorAll('.sky-count-btn')[0];
      var plus = row.querySelectorAll('.sky-count-btn')[1];
      var min = key === 'adult' ? 1 : 0;
      var max = key === 'adult' ? 9 : 8;

      minus.addEventListener('click', function () {
        if (counts[key] > min) {
          counts[key]--;
          countEl.textContent = counts[key];
          updateTravellerLabel();
        }
      });
      plus.addEventListener('click', function () {
        if (counts[key] < max) {
          counts[key]++;
          countEl.textContent = counts[key];
          updateTravellerLabel();
        }
      });
    });

    if (cabinSelect) {
      cabinSelect.addEventListener('change', updateTravellerLabel);
    }

    if (doneBtn) {
      doneBtn.addEventListener('click', function () {
        travellerDropdown.classList.remove('open');
      });
    }
  }

  // ── Sort dropdown ──
  var sortBtn = document.querySelector('.sort-btn');
  var sortDropdown = document.getElementById('sortDropdown');
  var sortLabel = document.getElementById('sortLabel');

  if (sortBtn && sortDropdown) {
    sortBtn.addEventListener('click', function (e) {
      e.stopPropagation();
      sortDropdown.classList.toggle('open');
    });

    sortDropdown.querySelectorAll('.sopt').forEach(function (opt) {
      opt.addEventListener('click', function (e) {
        e.stopPropagation();
        sortDropdown.querySelectorAll('.sopt').forEach(function (o) { o.classList.remove('active'); });
        opt.classList.add('active');
        sortLabel.textContent = opt.textContent;
        sortDropdown.classList.remove('open');
      });
    });
  }

  // ── Close dropdowns when clicking outside ──
  document.addEventListener('click', function () {
    if (tripTypeDropdown) tripTypeDropdown.classList.remove('open');
    if (travellerDropdown) travellerDropdown.classList.remove('open');
    if (sortDropdown) sortDropdown.classList.remove('open');
  });
});
