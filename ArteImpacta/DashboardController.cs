using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public DashboardController()
        {
            _context = new ImpactaContext();
            _context.Database.EnsureCreated();
        }

        // URL: GET api/dashboard/resumo
        [HttpGet("resumo")]
        public async Task<IActionResult> GetResumo()
        {
            // Agora consultamos as tabelas CORPORATIVAS
            int totalProjetos = await _context.ProjetosSociais.CountAsync();
            int totalIndicadores = await _context.Indicadores.CountAsync();
            int totalAcoes = await _context.Acoes.CountAsync();

            // Soma o total de "Pessoas Beneficiadas"
            double pessoasImpactadas = await _context.Acoes
                .Where(a => a.Indicador.Nome == "Pessoas Beneficiadas")
                .SumAsync(a => a.Valor);

            // Retorna os dados que a tela da Mariana precisa ler!
            return Ok(new
            {
                projetosAtivos = totalProjetos,
                pessoasImpactadas = pessoasImpactadas,
                totalAcoes = totalAcoes,
                scoreImpactoGeral = 85 // Um score fictício inicial
            });
        }
    }
}