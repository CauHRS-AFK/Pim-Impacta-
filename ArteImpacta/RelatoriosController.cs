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
    public class RelatoriosController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public RelatoriosController()
        {
            _context = new ImpactaContext();
        }

        // ROTA PARA GERAR O DOSSIÊ COMPLETO DE IMPACTO
        // URL: GET api/relatorios/geral
        [HttpGet("geral")]
        public async Task<IActionResult> GerarRelatorioGeral()
        {
            // O Entity Framework vai ao banco e faz a soma de todas as ações automaticamente!
            var relatorio = await _context.ProjetosSociais
                .Select(projeto => new {
                    NomeProjeto = projeto.Nome,
                    Descricao = projeto.Descricao,
                    
                    // Dentro de cada projeto, lista os indicadores e a SOMA TOTAL de impacto
                    Indicadores = projeto.Indicadores.Select(ind => new {
                        NomeIndicador = ind.Nome,
                        Unidade = ind.UnidadeMedida,
                        TotalAlcancado = ind.Acoes.Sum(a => a.Valor), // A mágica da consolidação de dados!
                        UltimaAcao = ind.Acoes.OrderByDescending(a => a.DataRegistro).Select(a => (DateTime?)a.DataRegistro).FirstOrDefault()
                    }).ToList()
                })
                .ToListAsync();

            return Ok(relatorio);
        }
    }
}