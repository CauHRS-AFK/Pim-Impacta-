using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InteligenciaController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public InteligenciaController()
        {
            _context = new ImpactaContext();
        }

        // 1. CÁLCULO DE SCORE DE IMPACTO (De 0 a 100)
        // URL: GET api/inteligencia/score
        [HttpGet("score")]
        public async Task<IActionResult> CalcularScore()
        {
            var totalAcoes = await _context.Acoes.CountAsync();
            var projetosAtivos = await _context.ProjetosSociais.CountAsync();
            
            // Lógica simples: Base 50 + bônus por engajamento e quantidade de projetos
            // Capado em 100 (Ninguém tira mais que 100)
            int pontuacaoBase = 50;
            int bonusAcoes = totalAcoes * 5; 
            int bonusProjetos = projetosAtivos * 10;
            
            int scoreFinal = pontuacaoBase + bonusAcoes + bonusProjetos;
            if (scoreFinal > 100) scoreFinal = 100;

            return Ok(new { Score = scoreFinal });
        }

        // 2. COMPARAÇÃO ENTRE PROJETOS (Ranking de Performance)
        // URL: GET api/inteligencia/comparacao
        [HttpGet("comparacao")]
        public async Task<IActionResult> CompararProjetos()
        {
            // Agrupa as ações por projeto e soma o impacto total gerado por cada um
            var ranking = await _context.ProjetosSociais
                .Select(p => new {
                    NomeProjeto = p.Nome,
                    TotalImpacto = p.Indicadores.SelectMany(i => i.Acoes).Sum(a => a.Valor),
                    TotalAcoes = p.Indicadores.SelectMany(i => i.Acoes).Count()
                })
                .OrderByDescending(p => p.TotalImpacto) // Ordena do maior para o menor
                .ToListAsync();

            return Ok(ranking);
        }

        // 3. PREVISÃO SIMPLES DE MACHINE LEARNING (Tendência Linear / Média Móvel)
        // URL: GET api/inteligencia/previsao
        [HttpGet("previsao")]
        public async Task<IActionResult> PreverProximoMes()
        {
            // Pega as ações dos últimos 30 dias para entender a velocidade atual do projeto
            var dataCorte = DateTime.Now.AddDays(-30);
            
            var acoesRecentes = await _context.Acoes
                .Where(a => a.DataRegistro >= dataCorte)
                .ToListAsync();

            if (!acoesRecentes.Any())
            {
                return Ok(new { 
                    Mensagem = "Dados insuficientes para prever o futuro. Registre mais ações.",
                    PrevisaoImpacto = 0 
                });
            }

            // Matemática preditiva: Calcula a média diária de impacto e projeta para os próximos 30 dias
            double impactoTotalRecente = acoesRecentes.Sum(a => a.Valor);
            double mediaDiaria = impactoTotalRecente / 30.0;
            double projecaoProximoMes = mediaDiaria * 30; // Projeção para o mês seguinte

            return Ok(new {
                Mensagem = "Com base no ritmo de crescimento atual da empresa, esta é a projeção de impacto para os próximos 30 dias.",
                RitmoDiario = Math.Round(mediaDiaria, 2),
                PrevisaoImpacto = Math.Round(projecaoProximoMes, 2)
            });
        }
    }
}