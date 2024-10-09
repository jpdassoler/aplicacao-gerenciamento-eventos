using System.ComponentModel.DataAnnotations;

namespace EventManagerBackend.DTOs
{
    public class LoginDTO
    {
        [Required]
        [StringLength(15, ErrorMessage = "O nome de usuário não pode ter mais de 15 caracteres.")]
        public string Usuario { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "A senha não pode ter maiss de 60 caracteres.")]
        public string Senha { get; set; }
    }
}
