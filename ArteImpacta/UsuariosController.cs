using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;
using System.Threading.Tasks;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public UsuariosController()
        {
            _context = new ImpactaContext();
        }

        // 1. ROTA PARA ATUALIZAR O NOME E O E-MAIL
        // URL: PUT api/usuarios/perfil
        [HttpPut("perfil")]
        public async Task<IActionResult> AtualizarPerfil([FromBody] PerfilRequest dados)
        {
            // Procura o usuário no banco usando o e-mail atual dele como confirmação
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dados.EmailAtual);
            
            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado. Verifique se o e-mail atual está correto." });
            }

            // Substitui os dados velhos pelos novos
            usuario.Nome = dados.NovoNome;
            usuario.Email = dados.NovoEmail;
            
            await _context.SaveChangesAsync(); // Salva no banco SQLite

            return Ok(new { 
                mensagem = "Perfil atualizado com sucesso!", 
                novoNome = usuario.Nome 
            });
        }

        // 2. ROTA PARA TROCAR A SENHA COM SEGURANÇA
        // URL: PUT api/usuarios/senha
        [HttpPut("senha")]
        public async Task<IActionResult> AtualizarSenha([FromBody] SenhaRequest dados)
        {
            // Procura o usuário que tenha exatamente o e-mail e a senha atual informados
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dados.EmailAtual && u.Senha == dados.SenhaAtual);
            
            if (usuario == null)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha atual incorretos. Acesso negado." });
            }

            // Atualiza a senha
            usuario.Senha = dados.NovaSenha;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Sua senha foi atualizada com segurança!" });
        }
    }

    // Classes "Molde" para receber os dados do Front-end
    public class PerfilRequest
    {
        public string EmailAtual { get; set; }
        public string NovoNome { get; set; }
        public string NovoEmail { get; set; }
    }

    public class SenhaRequest
    {
        public string EmailAtual { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }
}