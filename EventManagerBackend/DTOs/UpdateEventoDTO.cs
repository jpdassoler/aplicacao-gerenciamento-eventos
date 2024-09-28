using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventManagerBackend.DTOs
{
    public class UpdateEventoDTO
    {
        public int? ID_Endereco { get; set; }

        [StringLength(50, ErrorMessage = "O nome do evento não pode ter mais de 50 caracteres.")]
        public string? Nome { get; set; }

        public DateTime? Data { get; set; }

        [StringLength(200, ErrorMessage = "A URL da imagem do banner não pode ter mais de 200 caracteres.")]
        public string? Banner { get; set; }

        [StringLength(200, ErrorMessage = "A descrição do evento não pode ter mais de 200 caracteres.")]
        public string? Descricao { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "O preço do ingresso deve ser positivo.")]
        public decimal? Preco_Ingresso { get; set; }

        [StringLength(200, ErrorMessage = "A URL do ingresso não pode ter mais de 200 caracteres.")]
        public string? URL_Ingresso { get; set; }
    }
}
