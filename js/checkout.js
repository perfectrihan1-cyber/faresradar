/* ═══ MOBILE MENU ═══ */
function toggleMobileMenu(){
  document.getElementById('mobileMenu').classList.toggle('show');
}

/* ═══ PASSENGER TABS ═══ */
function switchPassenger(idx){
  document.querySelectorAll('.passenger-panel').forEach((p,i)=>{
    p.classList.toggle('active',i===idx);
  });
  document.querySelectorAll('.ptab').forEach((t,i)=>{
    t.classList.toggle('active',i===idx);
  });
}

/* ═══ PAYMENT METHOD ═══ */
let selectedPayment = 'card';
function selectPayment(type){
  selectedPayment = type;
  ['card','paypal'].forEach(t=>{
    document.getElementById('payopt_'+t).classList.toggle('selected', t===type);
    document.getElementById('panel_'+t).style.display = t===type ? 'block' : 'none';
  });
}

/* ═══ CARD FORMATTING ═══ */
function formatCard(el){
  let v = el.value.replace(/\D/g,'').substring(0,16);
  el.value = v.replace(/(.{4})/g,'$1 ').trim();
  clearError('cardnum','grp_cardnum');
  detectCardType(el);
}
function detectCardType(el){
  const v = el.value.replace(/\s/g,'');
  let icon = '💳';
  if(/^4/.test(v)) icon = '💙'; // Visa
  else if(/^5[1-5]/.test(v)) icon = '🟠'; // Mastercard
  else if(/^3[47]/.test(v)) icon = '🟢'; // Amex
  document.getElementById('cardIcon').textContent = icon;
}
function formatExpiry(el){
  let v = el.value.replace(/\D/g,'');
  if(v.length>=2) v = v.substring(0,2)+'/'+v.substring(2,4);
  el.value = v;
  clearError('expiry','grp_expiry');
}

/* ═══ ADD-ONS ═══ */
const BASE = 11569;
const ADDON_PRICES = { addon_bag:2800, addon_seat:800, addon_ins:1499, addon_meal:650 };
let addonTotal = 0;
let couponDiscount = 0;

function updateAddons(){
  addonTotal = 0;
  Object.keys(ADDON_PRICES).forEach(id=>{
    if(document.getElementById(id).checked) addonTotal += ADDON_PRICES[id];
  });
  recalc();
}
function recalc(){
  const total = BASE + addonTotal - couponDiscount;
  document.getElementById('addonLine').style.display = addonTotal>0?'flex':'none';
  document.getElementById('addonAmt').textContent = '₹'+addonTotal.toLocaleString('en-IN');
  document.getElementById('discountLine').style.display = couponDiscount>0?'flex':'none';
  document.getElementById('discountAmt').textContent = '−₹'+couponDiscount.toLocaleString('en-IN');
  document.getElementById('totalAmt').textContent = '₹'+total.toLocaleString('en-IN');
  document.getElementById('btnTotal').textContent = '₹'+total.toLocaleString('en-IN');
}

/* ═══ COUPON ═══ */
const COUPONS = { FLY10: 0.10, SAVE500: 500, AFL20: 0.20 };
function applyCoupon(){
  const code = document.getElementById('couponInput').value.trim().toUpperCase();
  const msg = document.getElementById('couponMsg');
  const inp = document.getElementById('couponInput');
  const btn = document.getElementById('couponBtn');

  if(!code){ msg.className='coupon-msg error'; msg.textContent='Please enter a coupon code.'; return; }

  if(COUPONS[code]!==undefined){
    const val = COUPONS[code];
    couponDiscount = val < 1 ? Math.round((BASE+addonTotal)*val) : val;
    recalc();
    msg.className='coupon-msg success'; msg.textContent='✓ Coupon applied! Saved ₹'+couponDiscount.toLocaleString('en-IN');
    inp.classList.add('applied');
    btn.textContent='Remove';
    btn.onclick = removeCoupon;
  } else {
    msg.className='coupon-msg error'; msg.textContent='Invalid coupon code. Try FLY10 or AFL20.';
    inp.classList.remove('applied');
  }
}
function removeCoupon(){
  couponDiscount=0;
  document.getElementById('couponInput').value='';
  document.getElementById('couponInput').classList.remove('applied');
  document.getElementById('couponMsg').textContent='';
  document.getElementById('couponBtn').textContent='Apply';
  document.getElementById('couponBtn').onclick=applyCoupon;
  recalc();
}

/* ═══ VALIDATION ═══ */
function clearError(fieldId, grpId){
  document.getElementById(grpId).classList.remove('has-error');
}
function setError(grpId){
  document.getElementById(grpId).classList.add('has-error');
}
function val(id){ return (document.getElementById(id)||{}).value?.trim()||''; }
function chk(id){ return document.getElementById(id)?.checked; }

function validate(){
  let ok = true;

  // Passenger 1
  if(!val('fn_0')){ setError('grp_fn_0'); ok=false; }
  if(!val('ln_0')){ setError('grp_ln_0'); ok=false; }
  if(!val('dob_0')){ setError('grp_dob_0'); ok=false; }
  if(!val('gen_0')){ setError('grp_gen_0'); ok=false; }
  if(!val('nat_0')){ setError('grp_nat_0'); ok=false; }
  if(!val('pp_0')){ setError('grp_pp_0'); ok=false; }
  if(!val('ppexp_0')){ setError('grp_ppexp_0'); ok=false; }

  // Passenger 2
  if(!val('fn_1')){ setError('grp_fn_1'); ok=false; }
  if(!val('ln_1')){ setError('grp_ln_1'); ok=false; }
  if(!val('dob_1')){ setError('grp_dob_1'); ok=false; }
  if(!val('gen_1')){ setError('grp_gen_1'); ok=false; }
  if(!val('nat_1')){ setError('grp_nat_1'); ok=false; }
  if(!val('pp_1')){ setError('grp_pp_1'); ok=false; }
  if(!val('ppexp_1')){ setError('grp_ppexp_1'); ok=false; }

  // Contact
  const emailRe = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if(!emailRe.test(val('email'))){ setError('grp_email'); ok=false; }
  if(!val('phone')){ setError('grp_phone'); ok=false; }

  // Billing
  if(!val('addr')){ setError('grp_addr'); ok=false; }
  if(!val('city')){ setError('grp_city'); ok=false; }
  if(!val('country')){ setError('grp_country'); ok=false; }
  if(!val('zip')){ setError('grp_zip'); ok=false; }

  // Payment
  if(selectedPayment==='card'){
    const rawCard = val('cardnum').replace(/\s/g,'');
    if(rawCard.length<15){ setError('grp_cardnum'); ok=false; }
    if(!val('cardname')){ setError('grp_cardname'); ok=false; }
    if(!val('expiry')||val('expiry').length<5){ setError('grp_expiry'); ok=false; }
    if(!val('cvv')||val('cvv').length<3){ setError('grp_cvv'); ok=false; }
  }
  // paypal needs no extra validation

  // Terms
  if(!chk('terms')){
    document.getElementById('termsErr').style.display='block';
    ok=false;
  } else {
    document.getElementById('termsErr').style.display='none';
  }

  if(!ok){
    // scroll to first error
    const firstErr = document.querySelector('.has-error');
    if(firstErr) firstErr.scrollIntoView({behavior:'smooth',block:'center'});
  }
  return ok;
}

/* ═══ SUBMIT ═══ */
function submitCheckout(){
  if(!validate()) return;

  const btn = document.getElementById('payBtn');
  btn.classList.add('loading');
  btn.disabled = true;

  // simulate payment processing
  setTimeout(()=>{
    btn.classList.remove('loading');
    btn.disabled = false;
    const pnr = 'AFL'+Math.random().toString(36).substring(2,7).toUpperCase();
    document.getElementById('pnrCode').textContent = 'PNR: '+pnr;
    document.getElementById('successModal').classList.add('show');
  }, 2200);
}

function closeModal(){
  document.getElementById('successModal').classList.remove('show');
  // could redirect: window.location.href = 'flight_result.html';
}