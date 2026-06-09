using System.Collections.Generic;

namespace ImpactaPlus.Models
{
    public class Indicador
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Ex: Pessoas beneficiadas [cite: 3]
        public string UnidadeMedida { get; set; } // Ex: Quantidade, Litros, Kilos, Horas
        
        public int ProjetoSocialId { get; set; }
        public ProjetoSocial ProjetoSocial { get; set; }

        // Um indicador recebe várias ações ao longo do tempo
        public List<Acao> Acoes { get; set; } = new List<Acao>();
    }
}