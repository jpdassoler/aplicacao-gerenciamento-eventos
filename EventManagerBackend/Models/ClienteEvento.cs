using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagerBackend.Models
{
    [PrimaryKey(nameof(Usuario), nameof(ID_Evento))]
    [Table("Cliente_Evento")]
    public class ClienteEvento
    {
        public string Usuario { get; set; }
        public int ID_Evento { get; set; }
        [Required]
        public EnumIndComparecimento Ind_Comparecimento { get; set; }
    }
}
