using EventManagerBackend.DTOs;
using EventManagerBackend.Models;

namespace EventManagerBackend.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<Cliente> GetClienteByUsuario(string usuario);
        Task AddCliente(Cliente cliente);
        Task UpdateCliente(string usuario, UpdateClienteDTO cliente);
        Task DeleteCliente(string id);
    }
}
