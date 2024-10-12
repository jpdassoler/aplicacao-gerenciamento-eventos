using EventManagerBackend.DTOs;
using EventManagerBackend.Models;

namespace EventManagerBackend.Repositories
{
    public interface IClienteEventoRepository
    {
        Task<IEnumerable<ClienteEvento>> GetAllClienteEventos();
        Task<ClienteEvento?> GetClienteEventoById(string usuario, int idEvento);
        Task AddClienteEvento(ClienteEvento clienteEvento);
        Task UpdateClienteEvento(ClienteEvento clienteEvento);
        Task DeleteClienteEvento(string usuario, int idEvento);
        Task<IEnumerable<ClienteEventoDetalhesDTO>> GetClientesByEventoAndComparecimento(int idEvento, EnumIndComparecimento indComparecimento);

    }
}
