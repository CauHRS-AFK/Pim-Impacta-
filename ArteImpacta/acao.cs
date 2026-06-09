using System;

namespace ImpactaPlus.Models
{
    public class Acao
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public double Valor { get; set; } // O número que foi alcançado na ação
        public string Descricao { get; set; } 
        
        // O sistema deve permitir registrar ações vinculadas a indicadores [cite: 86]
        public int IndicadorId { get; set; }
        public Indicador Indicador { get; set; }
    }
}