using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public ProjetosController()
        {
            _context = new ImpactaContext();
        }

        // 1. ROTA PARA CADASTRAR PROJETO (Sprint 2 - Requisito Funcional)
        // URL: POST api/projetos/cadastrar
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] ProjetoRequest dados)
        {
            // Validação simples
            if (string.IsNullOrEmpty(dados.Nome) || string.IsNullOrEmpty(dados.Descricao))
            {
                return BadRequest(new { mensagem = "O nome e a descrição do projeto são obrigatórios." });
            }

            // Verifica se a empresa informada existe de facto no banco
            var empresaExiste = await _context.Empresas.AnyAsync(e => e.Id == dados.EmpresaId);
            if (!empresaExiste)
            {
                return BadRequest(new { mensagem = "Empresa não encontrada. Não é possível vincular o projeto." });
            }

            // Cria o objeto do modelo baseado nos dados recebidos do Front-end
            var novoProjeto = new ProjetoSocial
            {
                Nome = dados.Nome,
                Descricao = dados.Descricao,
                EmpresaId = dados.EmpresaId
            };

            // Adiciona e grava no SQLite
            _context.ProjetosSociais.Add(novoProjeto);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Projeto '{novoProjeto.Nome}' cadastrado com sucesso!" });
        }

        // 2. ROTA PARA LISTAR PROJETOS (Sprint 2 - Requisito Funcional)
        // URL: GET api/projetos/listar
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            // Procura todos os projetos sociais no banco de dados
            var listaProjetos = await _context.ProjetosSociais
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Descricao,
                    p.EmpresaId
                })
                .ToListAsync();

            return Ok(listaProjetos);
        }
    }

    // Classe auxiliar (DTO) para receber o JSON do Front-end de forma organizada
    public class ProjetoRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int EmpresaId { get; set; } // Vincula o projeto à empresa dona do perfil
    }
}