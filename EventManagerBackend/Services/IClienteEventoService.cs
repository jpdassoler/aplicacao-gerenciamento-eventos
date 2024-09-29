using EventManagerBackend.DTOs;
using EventManagerBackend.Models;

namespace EventManagerBackend.Services
{
    public interface IClienteEventoService
    {
        Task<IEnumerable<ClienteEvento>> GetAllClienteEventos();
        Task<ClienteEvento?> GetClienteEventoById(string usuario, int idEvento);
        Task AddClienteEvento(ClienteEvento clienteEvento);
        Task UpdateClienteEvento(string usuario, int idEvento, UpdateClienteEventoDTO clienteEvento);
        Task DeleteClienteEvento(string usuario, int idEvento);
    }
}
