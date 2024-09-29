using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagerBackend.Models
{
    [Table("ClienteEvento")]
    public class ClienteEvento
    {
        [Key, Column(Order = 0)]
        public string Usuario { get; set; }

        [Key, Column(Order = 1)]
        public int ID_Evento { get; set; }

        [Required]
        public EnumIndComparecimento Ind_Comparecimento { get; set; }

        [ForeignKey("Usuario")]
        public Cliente Cliente { get; set; }

        [ForeignKey("ID_Evento")]
        public Evento Evento { get; set; }
    }
}
