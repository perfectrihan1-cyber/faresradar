// ═══ STATE ═══
let tripType = 'return';
let counts = {adult:1, child:0, infant:0};
let cabin = 'Economy';

// ═══ NAV ═══
function setNavTab(btn, type){
  document.querySelectorAll('.ntab').forEach(b=>b.classList.remove('active'));
  btn.classList.add('active');
}
function toggleMobileMenu(){
  document.getElementById('mobileMenu').classList.toggle('open');
}

// ═══ TRIP TYPE ═══
function toggleTripType(){
  document.getElementById('tripTypeDD').classList.toggle('open');
  event.stopPropagation();
}
function setTripType(type, el){
  tripType = type;
  const labels = {return:'↩ Return', one:'→ One way', multi:'⇄ Multi-city'};
  document.getElementById('tripTypeLabel').textContent = labels[type];
  document.querySelectorAll('.tt-opt').forEach(o=>o.classList.remove('selected'));
  el.classList.add('selected');
  document.getElementById('tripTypeDD').classList.remove('open');
  // Show/hide return field
  const rf = document.getElementById('returnField');
  if(type === 'one') rf.style.display='none';
  else rf.style.display='';
}

// ═══ TRAVELLERS ═══
function toggleTravellers(){
  document.getElementById('trDropdown').classList.toggle('open');
}
function closeTravellers(){
  document.getElementById('trDropdown').classList.remove('open');
  updateTravellerLabel();
}
function changeCount(type, delta){
  event.stopPropagation();
  counts[type] = Math.max(0, counts[type] + delta);
  if(counts.adult < 1) counts.adult = 1;
  document.getElementById('adultCount').textContent = counts.adult;
  document.getElementById('childCount').textContent = counts.child;
  document.getElementById('infantCount').textContent = counts.infant;
  updateTravellerLabel();
}
function updateCabin(){
  cabin = document.getElementById('cabinSel').value;
  updateTravellerLabel();
}
function updateTravellerLabel(){
  const total = counts.adult + counts.child + counts.infant;
  const pax = total === 1 ? '1 Adult' : `${total} Travellers`;
  document.getElementById('travellerDisplay').textContent = `${pax}, ${cabin}`;
}

// ═══ SWAP AIRPORTS ═══
function swapAirports(){
  const from = document.getElementById('fromInput');
  const to = document.getElementById('toInput');
  [from.value, to.value] = [to.value, from.value];
}

// ═══ DESTINATION TABS ═══
function switchDestTab(btn, tab){
  document.querySelectorAll('.dest-tab').forEach(b=>b.classList.remove('active'));
  btn.classList.add('active');
  document.getElementById('intlGrid').style.display = tab==='intl'?'grid':'none';
  document.getElementById('domGrid').style.display = tab==='dom'?'grid':'none';
}

// ═══ SEARCH ═══
function doSearch(){
  const from = document.getElementById('fromInput').value;
  const to = document.getElementById('toInput').value;
  if(!from.trim() || !to.trim()){showToast('Please enter origin and destination');return;}
  showToast(`Searching flights: ${from} → ${to}`);
}
function searchRoute(from, to){
  document.getElementById('fromInput').value = from;
  document.getElementById('toInput').value = to;
  showToast(`Searching: ${from} → ${to}...`);
}

// ═══ FAQ ═══
function toggleFaq(el){
  const wasOpen = el.classList.contains('open');
  document.querySelectorAll('.faq-item').forEach(i=>i.classList.remove('open'));
  if(!wasOpen) el.classList.add('open');
}

// ═══ TOAST ═══
function showToast(msg){
  const t = document.getElementById('toast');
  t.textContent = msg;
  t.classList.add('show');
  setTimeout(()=>t.classList.remove('show'), 2800);
}

// ═══ CLOSE DROPDOWNS ON OUTSIDE CLICK ═══
document.addEventListener('click', e=>{
  if(!e.target.closest('#tripTypeBtn')) document.getElementById('tripTypeDD').classList.remove('open');
  if(!e.target.closest('#travellersBtn')) document.getElementById('trDropdown').classList.remove('open');
  if(!e.target.closest('.mobile-menu') && !e.target.closest('.hamburger')) document.getElementById('mobileMenu').classList.remove('open');
});
