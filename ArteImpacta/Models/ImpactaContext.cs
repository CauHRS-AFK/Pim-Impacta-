using Microsoft.EntityFrameworkCore;

namespace ImpactaPlus.Models
{
    public class ImpactaContext : DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ProjetoSocial> ProjetosSociais { get; set; }
        public DbSet<Indicador> Indicadores { get; set; }
        public DbSet<Acao> Acoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=impacta_corporativo.db");
        }
    }
}