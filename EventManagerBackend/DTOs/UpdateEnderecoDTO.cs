using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagerBackend.DTOs
{
    public class UpdateEnderecoDTO
    {
        [Range(10000000, 99999999, ErrorMessage = "O CEP deve ter 8 dígitos.")]
        public int CEP { get; set; }

        [StringLength(200, ErrorMessage = "O nome da rua não pode ter mais de 200 caracteres.")]
        public string Rua { get; set; }
        public int? Numero { get; set; }

        [StringLength(200, ErrorMessage = "O complemento não pode ter mais de 200 caracteres.")]
        public string? Complemento { get; set; }

        [StringLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        public string Bairro { get; set; }

        [StringLength(100, ErrorMessage = "A cidade não pode ter mais de 100 caracteres.")]
        public string Cidade { get; set; }

        [StringLength(2, ErrorMessage = "O estado não pode ter mais de 2 caracteres.")]
        public string UF { get; set; }
    }
}
