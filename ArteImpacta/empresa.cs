using System.Collections.Generic;

namespace ImpactaPlus.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        
        // Uma empresa tem vários usuários e vários projetos
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<ProjetoSocial> Projetos { get; set; } = new List<ProjetoSocial>();
    }
}