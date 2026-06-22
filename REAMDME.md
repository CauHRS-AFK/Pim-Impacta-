# 🌱 Impacta+

> Plataforma Corporativa de Gestão de Impacto Social e Conformidade ESG.

O **Impacta+** é um sistema web full-stack desenvolvido para gerenciar, registrar e consolidar iniciativas de impacto social. A plataforma permite que gestores ESG e instituições acompanhem o progresso de projetos (como Inclusão Digital e Reflorestamento Urbano) através de um dashboard dinâmico e relatórios automatizados, garantindo transparência e rastreabilidade dos dados.

## 🚀 Funcionalidades

* **Dashboard Dinâmico:** Visualização em tempo real de indicadores, gráficos de distribuição e score de impacto consumindo dados direto da API.
* **Gestão de Projetos Sociais:** Estruturação de projetos e seus respectivos indicadores de sucesso (relacionamento relacional no banco de dados).
* **Registro de Ações em Campo:** Formulário de alimentação de dados com validação de regras de negócio.
* **Geração de Relatórios:** Consolidação automatizada de dados (Dossiê ESG) processada no servidor para maior performance.
* **Autenticação e Segurança:** Sistema de login com validação de usuários.

## 💻 Tecnologias Utilizadas

**Front-end:**
* HTML5 & CSS3 (Design responsivo e acessível)
* JavaScript Vanilla (Manipulação de DOM e consumo assíncrono de endpoints via Fetch API)

**Back-end:**
* C# e .NET (Web API RESTful)
* Entity Framework Core (Mapeamento Objeto-Relacional - ORM)
* Padrão MVC (Model-View-Controller) para organização das rotas e regras de negócio

**Banco de Dados:**
* SQLite (Banco de dados relacional leve e embutido)

## ⚙️ Como executar o projeto localmente

### Pré-requisitos
* [.NET SDK](https://dotnet.microsoft.com/download) instalado na máquina.
* Extensão "Live Server" no VS Code (para rodar o Front-end).

### Passo a Passo

1. **Clone este repositório:**
   ```bash
   git clone [https://github.com/CauHRS-AFK/Pim-Impacta-.git](https://github.com/CauHRS-AFK/Pim-Impacta-.git)

Passo 2: Iniciar o Servidor Back-end (C# / API)
Abra a pasta do projeto no VS Code.

Abra o terminal integrado do VS Code (Ctrl + ').

Navegue até a pasta onde está o arquivo principal do C# (ex: ArteImpacta ou onde está o arquivo .csproj).

Execute o comando abaixo para baixar os pacotes e ligar o servidor:
dotnet run

Aguarde a mensagem no terminal indicando que a aplicação está rodando (geralmente em http://localhost:5000). Mantenha este terminal aberto!

Passo 3: Iniciar o Front-end (HTML/JS)
Com o servidor rodando em segundo plano, abra os arquivos HTML do projeto no VS Code.

Clique com o botão direito do mouse no arquivo index.html (ou login.html).

Selecione a opção "Open with Live Server".

O seu navegador padrão será aberto automaticamente com a aplicação rodando (geralmente em http://127.0.0.1:5500).

Pronto! Agora o Front-end conseguirá se comunicar perfeitamente com o Back-end e o banco de dados.
