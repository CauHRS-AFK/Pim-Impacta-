document.addEventListener('DOMContentLoaded', () => {
  // === INÍCIO DO PASSO B: CARREGAR O NOME DO GESTOR ===
  const nomeDoBanco = localStorage.getItem('nomeUsuarioLogado');

  if (nomeDoBanco) {
    // 1. Altera o nome no perfil do canto inferior esquerdo
    const perfilNome = document.querySelector('.user-profile h4');
    if (perfilNome) perfilNome.innerText = nomeDoBanco;

    // 2. Altera as iniciais do avatar (Ex: Mariana Carneiro vira "MC")
    const avatar = document.querySelector('.avatar');
    if (avatar) {
      const iniciais = nomeDoBanco
        .split(' ')
        .map(n => n[0])
        .join('')
        .substring(0, 2)
        .toUpperCase();
      avatar.innerText = iniciais;
    }

    // 3. Altera a mensagem de boas-vindas lá de cima
    const boasVindas = document.querySelector('.header-welcome h1');
    if (boasVindas) {
      const primeiroNome = nomeDoBanco.split(' ')[0];
      boasVindas.innerText = `Olá, ${primeiroNome}! 👋`;
    }
  }

  // As rotas da nossa API
  const API_RESUMO = 'http://localhost:5000/api/dashboard/resumo';
  const API_SCORE = 'http://localhost:5000/api/inteligencia/score';
  const API_COMPARACAO = 'http://localhost:5000/api/inteligencia/comparacao';
  const API_PREVISAO = 'http://localhost:5000/api/inteligencia/previsao';

  // 1. CARREGAR OS DADOS BÁSICOS (Pessoas, Projetos, Ações)
  fetch(API_RESUMO)
    .then(res => res.json())
    .then(dados => {
      document.getElementById('numero-pessoas').innerText =
        dados.pessoasImpactadas.toLocaleString('pt-BR');
      document.getElementById('numero-projetos').innerText =
        dados.projetosAtivos;
      document.getElementById('numero-acoes').innerText = dados.totalAcoes;
      document.getElementById('donut-total-projetos').innerText =
        dados.projetosAtivos;

      // Gráfico de linha (Evolução estática para visualização)
      const ctxFreq = document
        .getElementById('frequencyChart')
        .getContext('2d');
      const gradient = ctxFreq.createLinearGradient(0, 0, 0, 200);
      gradient.addColorStop(0, 'rgba(34, 197, 94, 0.24)');
      gradient.addColorStop(1, 'rgba(34, 197, 94, 0.0)');

      new Chart(ctxFreq, {
        type: 'line',
        data: {
          labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
          datasets: [
            {
              label: 'Índice de Impacto',
              data: [30, 45, 55, 70, 85, dados.pessoasImpactadas > 0 ? 95 : 0],
              borderColor: '#16A34A',
              borderWidth: 3,
              pointBackgroundColor: '#16A34A',
              fill: true,
              backgroundColor: gradient,
              tension: 0.3,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: { legend: { display: false } },
        },
      });
    });

  // 2. CARREGAR O SCORE DE IMPACTO (Inteligência)
  fetch(API_SCORE)
    .then(res => res.json())
    .then(dados => {
      document.getElementById('numero-score').innerText = dados.score;

      const ctxGauge = document.getElementById('gaugeChart').getContext('2d');
      new Chart(ctxGauge, {
        type: 'doughnut',
        data: {
          datasets: [
            {
              data: [dados.score, 100 - dados.score],
              backgroundColor: ['#22C55E', '#E2E8F0'],
              borderWidth: 0,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: { tooltip: { enabled: false } },
          cutout: '80%',
          rotation: -90,
          circumference: 180,
        },
      });
    });

  // 3. CARREGAR COMPARAÇÃO DE PROJETOS (Gráfico de Rosca)
  fetch(API_COMPARACAO)
    .then(res => res.json())
    .then(projetos => {
      const nomes = projetos.map(p => p.nomeProjeto);
      const impactos = projetos.map(p => p.totalImpacto);

      // Cores para os diferentes projetos
      const cores = ['#22C55E', '#3B82F6', '#F59E0B', '#8B5CF6', '#EC4899'];

      const ctxCourse = document.getElementById('courseChart').getContext('2d');
      new Chart(ctxCourse, {
        type: 'doughnut',
        data: {
          labels: nomes.length > 0 ? nomes : ['Sem Projetos'],
          datasets: [
            {
              data:
                impactos.length > 0 && impactos.some(i => i > 0)
                  ? impactos
                  : [1],
              backgroundColor: impactos.some(i => i > 0) ? cores : ['#E2E8F0'],
              borderWidth: 0,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: 'right',
              labels: { boxWidth: 12, font: { size: 11 } },
            },
          },
          cutout: '75%',
        },
      });
    });

  // 4. CARREGAR PREVISÃO (Machine Learning)
  fetch(API_PREVISAO)
    .then(res => res.json())
    .then(dados => {
      // Procura a tag que tem o alerta no HTML e injeta a mensagem do C# nela!
      const alertaElemento = document.querySelector('.vis-footer-alert span');
      if (alertaElemento) {
        alertaElemento.innerHTML = `🤖 <strong>Previsão de IA:</strong> ${dados.mensagem} <br>📈 Projeção para 30 dias: <strong>+${dados.previsaoImpacto} de impacto gerado</strong>.`;
      }
    });
});
