using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagerBackend.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        [Required]
        [StringLength(15, ErrorMessage = "O nome de usuário não pode ter mais de 15 caracteres.")]
        public string Usuario { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "A senha não pode ter maiss de 60 caracteres.")]
        public string Senha { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }
        [EmailAddress(ErrorMessage = "O formato de e-mail é inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail não pode ter mais de 100 caracteres.")]
        public string Email { get; set; }
        [StringLength(11, ErrorMessage = "O telefone não pode ter mais de 11 números.")]
        public string Telefone { get; set; }
        [StringLength(50, ErrorMessage = "O usuário do Instagram não pode ter mais de 50 caracteres.")]
        public string Instagram { get; set; }
    }
}
