namespace ImpactaPlus.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; } // O sistema deve permitir login e autenticação [cite: 16]
        
        // Relacionamento: O sistema deve vincular usuários a uma empresa [cite: 20]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}