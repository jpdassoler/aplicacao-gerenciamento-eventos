using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagerBackend.Models
{
    [Table("Endereco")]
    public class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Endereco { get; set; }  
        
        [Required]
        [Range(10000000, 99999999, ErrorMessage = "O CEP deve ter 8 dígitos.")]
        public int CEP { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "O nome da rua não pode ter mais de 200 caracteres.")]
        public string Rua { get; set; }
        public int? Numero { get; set; }

        [StringLength(200, ErrorMessage = "O complemento não pode ter mais de 200 caracteres.")]
        public string? Complemento { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        public string Bairro { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A cidade não pode ter mais de 100 caracteres.")]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "O estado não pode ter mais de 2 caracteres.")]
        public string UF { get; set; }
    }
}
