using EventManagerBackend.Models;

namespace EventManagerBackend.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<Cliente> GetClienteById(string id);
        Task AddCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente);
        Task DeleteCliente(string id);
    }
}
