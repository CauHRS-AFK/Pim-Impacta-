using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImpactaPlus.Models;
using System.Threading.Tasks;

namespace ImpactaPlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ImpactaContext _context;

        public LoginController()
        {
            _context = new ImpactaContext();
        }

        [HttpPost("entrar")]
        public async Task<IActionResult> Entrar([FromBody] LoginRequest dados)
        {
            // Vai no banco de dados e procura alguém com esse e-mail e essa senha exata
            var usuarioExiste = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dados.Email && u.Senha == dados.Senha);

            // Se não achou ninguém (veio nulo), barra o acesso
            if (usuarioExiste == null)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha incorretos. Tente novamente." });
            }

            // Se achou, libera a entrada e manda o nome da pessoa para dar "Olá" na tela
            return Ok(new { 
                mensagem = "Login realizado com sucesso!",
                nomeUsuario = usuarioExiste.Nome 
            });
        }

        // NOVA ROTA: RECUPERAÇÃO DE SENHA (Simulação para o projeto)
        // URL: POST api/login/recuperar-senha
        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarRequest dados)
        {
            if (string.IsNullOrEmpty(dados.Email))
            {
                return BadRequest(new { mensagem = "O e-mail é obrigatório." });
            }

            var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dados.Email);

            if (usuarioExiste == null)
            {
                // Em sistemas reais, por segurança, às vezes não avisamos se o e-mail não existe.
                // Mas para o projeto acadêmico, é bom mostrar o erro na tela.
                return NotFound(new { mensagem = "Este e-mail não foi encontrado na nossa base de dados." });
            }

            // Simula o disparo do e-mail
            return Ok(new { 
                mensagem = $"As instruções de redefinição foram enviadas para {dados.Email}. (Simulação ativada)" 
            });
        }
    } // Fim da classe LoginController

    // O "Molde" para receber o e-mail da tela de Esqueci a Senha
    public class RecuperarRequest
    {
        public string Email { get; set; }
    }
    }

    // O "Molde" para receber os dados do Front-end
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    
