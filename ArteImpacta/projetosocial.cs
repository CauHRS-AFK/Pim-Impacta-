using System.Collections.Generic;

namespace ImpactaPlus.Models
{
    public class ProjetoSocial
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        
        // Relacionamento com Empresa
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        // O sistema deve permitir associar indicadores aos projetos [cite: 27]
        public List<Indicador> Indicadores { get; set; } = new List<Indicador>();
    }
}