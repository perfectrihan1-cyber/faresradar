function toggleMobileMenu() {
  const m = document.getElementById('mobileMenu');
  m.classList.toggle('open');
}

function selectSite(site) {
  const names = {
    parallelfares: 'ParallelFares.com — Redirecting to checkout…',
    faresbridge:   'FaresBridge.com — Redirecting to checkout…',
    faresgator:    'FaresGator.com — Redirecting to checkout…'
  };
  sendPrompt('I selected ' + names[site]);
}
