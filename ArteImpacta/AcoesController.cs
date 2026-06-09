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
    public class AcoesController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public AcoesController()
        {
            _context = new ImpactaContext();
        }

        // 1. ROTA PARA LISTAR INDICADORES (Preenche o menu de opções no Front-end)
        // URL: GET api/acoes/indicadores
        [HttpGet("indicadores")]
        public async Task<IActionResult> GetIndicadores()
        {
            // Busca os indicadores e já traz o nome do projeto junto para ficar claro na tela
            var lista = await _context.Indicadores
                .Include(i => i.ProjetoSocial)
                .Select(i => new {
                    id = i.Id,
                    nome = i.Nome,
                    unidade = i.UnidadeMedida,
                    projetoNome = i.ProjetoSocial.Nome
                })
                .ToListAsync();

            return Ok(lista);
        }

        // 2. ROTA PARA REGISTRAR UMA NOVA AÇÃO (O impacto acontecendo na prática)
        // URL: POST api/acoes/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarAcao([FromBody] AcaoRequest dados)
        {
            // Validação de segurança básica
            if (dados.Valor <= 0)
            {
                return BadRequest(new { mensagem = "O valor numérico do impacto deve ser maior que zero." });
            }

            var indicadorExiste = await _context.Indicadores.AnyAsync(i => i.Id == dados.IndicadorId);
            if (!indicadorExiste)
            {
                return BadRequest(new { mensagem = "Indicador não encontrado no sistema." });
            }

            // Monta a ação que será salva no banco
            var novaAcao = new Acao
            {
                IndicadorId = dados.IndicadorId,
                Valor = dados.Valor,
                Descricao = dados.Descricao,
                // Se o front-end não mandar data, o C# pega a data e hora exata de agora
                DataRegistro = dados.DataRegistro == default ? DateTime.Now : dados.DataRegistro
            };

            // Salva de verdade no SQLite
            _context.Acoes.Add(novaAcao);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ação registrada com sucesso! O impacto já está a ser contabilizado no Dashboard." });
        }
    }

    // Classe "Molde" (DTO) para receber os dados do JavaScript
    public class AcaoRequest
    {
        public int IndicadorId { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}