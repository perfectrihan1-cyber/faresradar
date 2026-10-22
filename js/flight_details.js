// ── Mobile menu ──
function toggleMobileMenu(){
  document.getElementById('mobileMenu').classList.toggle('show');
}
document.addEventListener('click', e => {
  if(!e.target.closest('.hamburger') && !e.target.closest('.mobile-menu'))
    document.getElementById('mobileMenu').classList.remove('show');
});

// ── Accordion ──
function toggleAcc(header){
  const body = header.nextElementSibling;
  const arrow = header.querySelector('.acc-arrow');
  body.classList.toggle('open');
  arrow.classList.toggle('open');
}

// ── Segment tabs ──
function switchTab(btn, panelId){
  const tabs = btn.closest('.seg-tabs').querySelectorAll('.seg-tab');
  const panels = btn.closest('.dcard').querySelectorAll('.seg-panel');
  tabs.forEach(t => t.classList.remove('active'));
  panels.forEach(p => p.classList.remove('active'));
  btn.classList.add('active');
  document.getElementById(panelId).classList.add('active');
}

// ── Toast ──
let toastTimer;
function showToast(msg){
  const t = document.getElementById('toast');
  t.textContent = msg;
  t.classList.add('show');
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => t.classList.remove('show'), 2800);
}

// ── Book handler ──
function handleBook(e){
  if(e) e.preventDefault();
  showToast('Redirecting to checkout…');
}

// ── Generate alternative flights ──
const altFlights = [
  { logo:'SG', bg:'linear-gradient(135deg,#f59e0b,#b45309)', name:'SpiceJet', flight:'SG-123', dep:'07:15', arr:'09:25', dur:'2h 10m', stops:'Nonstop', price:'₹7,299', diff:'-₹1,200', cheaper:true },
  { logo:'UK', bg:'linear-gradient(135deg,#7c3aed,#4c1d95)', name:'Vistara', flight:'UK-956', dep:'10:00', arr:'12:10', dur:'2h 10m', stops:'Nonstop', price:'₹9,150', diff:'+₹651', cheaper:false },
  { logo:'G8', bg:'linear-gradient(135deg,#059669,#065f46)', name:'GoAir', flight:'G8-332', dep:'15:45', arr:'18:25', dur:'2h 40m', stops:'1 stop', price:'₹6,899', diff:'-₹1,600', cheaper:true },
];

const altWrap = document.getElementById('altFlights');
altFlights.forEach(f => {
  altWrap.innerHTML += `
    <div class="alt-flight-card" onclick="showToast('${f.name} flight selected!')">
      <div class="alt-airline-logo" style="background:${f.bg}">${f.logo}</div>
      <div class="alt-info">
        <div class="alt-route">${f.dep} → ${f.arr} · <strong>${f.name}</strong></div>
        <div class="alt-meta">${f.flight} · ${f.dur} · ${f.stops}</div>
      </div>
      <div class="alt-price-col">
        <div class="alt-price">${f.price}</div>
        <div class="alt-diff ${f.cheaper ? 'cheaper':''}">${f.diff}</div>
      </div>
      <button class="alt-select-btn">Select</button>
    </div>`;
});