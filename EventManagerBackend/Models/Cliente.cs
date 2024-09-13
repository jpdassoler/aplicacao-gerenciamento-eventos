using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagerBackend.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Instagram { get; set; }
    }
}
