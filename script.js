document.addEventListener('DOMContentLoaded', () => {
  const togglePasswordBtn = document.getElementById('togglePassword');
  const passwordInput = document.getElementById('password');

  togglePasswordBtn.addEventListener('click', () => {
    // Verifica o tipo atual do input
    const type =
      passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);

    // Alterna o ícone (você pode trocar por SVGs reais de olho aberto/fechado)
    if (type === 'text') {
      togglePasswordBtn.textContent = '🙈';
    } else {
      togglePasswordBtn.textContent = '👁️';
    }
  });
});
