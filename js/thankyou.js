/* ─── Mobile Menu ─── */
function toggleMobileMenu(){
  document.getElementById('mobileMenu').classList.toggle('show');
}

/* ─── Copy PNR ─── */
function copyPNR(){
  const copyImgHtml = '<img src="./images/copy.png" alt="Copy" style="width:14px;height:14px;vertical-align:middle;margin-right:4px;"> Copy';
  const copiedImgHtml = '<img src="./images/Check-Your-Email.png" alt="Copied" style="width:14px;height:14px;vertical-align:middle;margin-right:4px;"> Copied!';
  const pnr = document.getElementById('pnrDisplay').textContent;
  navigator.clipboard.writeText(pnr).then(()=>{
    const btn = document.getElementById('copyBtn');
    btn.innerHTML = copiedImgHtml;
    setTimeout(()=>{ btn.innerHTML = copyImgHtml; },2000);
  }).catch(()=>{
    // fallback
    const ta = document.createElement('textarea');
    ta.value = pnr;
    document.body.appendChild(ta);
    ta.select();
    document.execCommand('copy');
    document.body.removeChild(ta);
    const btn = document.getElementById('copyBtn');
    btn.innerHTML = copiedImgHtml;
    setTimeout(()=>{ btn.innerHTML = copyImgHtml; },2000);
  });
}

/* ─── Confetti ─── */
function launchConfetti(){
  const colors = ['#0770e3','#00875a','#ff6600','#f7c948','#e53e3e','#00c97a','#a855f7'];
  const wrap = document.getElementById('confettiWrap');
  for(let i=0;i<80;i++){
    const el = document.createElement('div');
    el.className = 'confetti-piece';
    const size = Math.random()*10+6;
    el.style.cssText = `
      left:${Math.random()*100}%;
      top:-${size}px;
      width:${size}px;
      height:${size}px;
      background:${colors[Math.floor(Math.random()*colors.length)]};
      border-radius:${Math.random()>0.5?'50%':'3px'};
      animation-duration:${Math.random()*2+1.5}s;
      animation-delay:${Math.random()*1}s;
    `;
    wrap.appendChild(el);
    el.addEventListener('animationend',()=>el.remove());
  }
}

window.addEventListener('load', ()=>{
  // small delay for dramatic effect
  setTimeout(launchConfetti, 300);
});