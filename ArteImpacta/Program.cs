using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ImpactaPlus.Models; 

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAÇÃO DOS SERVIÇOS (O que a API sabe fazer)
builder.Services.AddControllers();

// Mantendo o CORS ativado para o Front-end não ter bloqueios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontEnd",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// 2. CONFIGURAÇÃO DO FLUXO 
app.UseCors("PermitirFrontEnd");
app.UseAuthorization();
app.MapControllers();

// 3. SEED DO BANCO DE DADOS (Popula o banco corporativo na primeira vez que rodar)
Console.WriteLine("=== IMPACTA+ | Inicializando Banco de Dados Corporativo ===\n");

using (var db = new ImpactaContext())
{
    // Cria o banco de dados caso não exista
    db.Database.EnsureCreated();

    // Verifica se já existe alguma empresa para não duplicar dados
    if (!db.Empresas.Any())
    {
        Console.WriteLine("Banco vazio! Injetando dados fictícios do modelo SaaS...\n");

        // Criando a Empresa e a Usuária Gestora
        var empresa = new Empresa { NomeFantasia = "Tech do Bem", CNPJ = "12.345.678/0001-90" };
        var usuariaMariana = new Usuario { Nome = "Mariana Silva", Email = "mariana@techdobem.org", Senha = "senha_segura_123", Empresa = empresa };

        // Criando um Projeto Social vinculado à Empresa
        var projetoEducacao = new ProjetoSocial { Nome = "Inclusão Digital", Descricao = "Levando acesso à tecnologia para comunidades", Empresa = empresa };

        // Criando os Indicadores que o projeto vai medir
        var indPessoas = new Indicador { Nome = "Pessoas Beneficiadas", UnidadeMedida = "Pessoas", ProjetoSocial = projetoEducacao };
        var indEquipamentos = new Indicador { Nome = "Equipamentos Doados", UnidadeMedida = "Unidades", ProjetoSocial = projetoEducacao };

        // Registrando algumas Ações (O impacto real acontecendo)
        var acao1 = new Acao { DataRegistro = DateTime.Now.AddDays(-15), Valor = 1250, Descricao = "Alunos formados no semestre", Indicador = indPessoas };
        var acao2 = new Acao { DataRegistro = DateTime.Now.AddDays(-5), Valor = 380, Descricao = "Notebooks entregues", Indicador = indEquipamentos };

        // Adicionando tudo ao banco
        db.Empresas.Add(empresa);
        db.Usuarios.Add(usuariaMariana);
        db.ProjetosSociais.Add(projetoEducacao);
        db.Indicadores.Add(indPessoas);
        db.Indicadores.Add(indEquipamentos);
        db.Acoes.Add(acao1);
        db.Acoes.Add(acao2);

        db.SaveChanges(); // Salva de verdade no SQLite
        Console.WriteLine("Dados corporativos e ESG salvos com sucesso!\n");
    }
    else
    {
        Console.WriteLine("Banco de dados já contém registros. Pronto para uso.\n");
    }
}

// 4. LIGA O SERVIDOR WEB API
Console.WriteLine("Servidor Web API Ativo! Aguardando conexões do Front-end...");
app.Run();