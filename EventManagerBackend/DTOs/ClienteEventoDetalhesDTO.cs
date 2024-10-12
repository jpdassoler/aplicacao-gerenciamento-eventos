using EventManagerBackend.Models;

namespace EventManagerBackend.DTOs
{
    public class ClienteEventoDetalhesDTO
    {
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public EnumIndComparecimento IndComparecimento { get; set; }

    }
}
