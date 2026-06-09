using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;
using System.Threading.Tasks;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadastroController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public CadastroController()
        {
            _context = new ImpactaContext();
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] CadastroRequest dados)
        {
            // 1. Validação básica de segurança
            if (await _context.Empresas.AnyAsync(e => e.CNPJ == dados.Cnpj))
            {
                return BadRequest(new { mensagem = "Já existe uma empresa cadastrada com este CNPJ no sistema." });
            }

            // 2. Cria a nova Empresa no banco
            var novaEmpresa = new Empresa
            {
                NomeFantasia = dados.EmpresaNome,
                CNPJ = dados.Cnpj
            };

            _context.Empresas.Add(novaEmpresa);
            
            // O SaveChanges aqui é crucial! Ele gera o ID da empresa que acabamos de criar.
            await _context.SaveChangesAsync(); 

            // 3. Cria o Usuário (Gestor) e vincula ao ID da Empresa criada
            var novoUsuario = new Usuario
            {
                Nome = dados.Nome,
                Email = dados.Email,
                Senha = dados.Senha, // No mercado real, usaríamos um Hash aqui!
                EmpresaId = novaEmpresa.Id 
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            // 4. Devolve a confirmação de sucesso para o Front-end
            return Ok(new { mensagem = "Conta corporativa criada com sucesso no Impacta+!" });
        }
    }

    // Classe "Molde" (DTO) para receber exatamente os campos que vêm do JavaScript
    public class CadastroRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string EmpresaNome { get; set; }
        public string Cnpj { get; set; }
    }
}