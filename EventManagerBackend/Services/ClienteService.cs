using EventManagerBackend.Models;
using EventManagerBackend.Repositories;

namespace EventManagerBackend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository) 
        { 
            _clienteRepository = clienteRepository;
        }
        public async Task<IEnumerable<Cliente>> GetAllClientes() 
        {
            return await _clienteRepository.GetAllClientes();
        }
        public async Task<Cliente> GetClienteById(string id) 
        {
            return await _clienteRepository.GetClienteById(id);
        }
        public async Task AddCliente(Cliente cliente)
        {
            if (String.IsNullOrEmpty(cliente.Usuario))
            {
                throw new ArgumentException("Usuário é obrigatório");
            }
            if (String.IsNullOrEmpty(cliente.Senha))
            {
                throw new ArgumentException("Senha é obrigatória");
            }
            if (String.IsNullOrEmpty(cliente.Nome))
            {
                throw new ArgumentException("Nome é obrigatório");
            }
            await _clienteRepository.AddCliente(cliente);
        }
        public async Task UpdateCliente(Cliente cliente)
        {
            if (String.IsNullOrEmpty(cliente.Usuario))
            {
                throw new ArgumentException("Usuário é obrigatório");
            }
            await _clienteRepository.UpdateCliente(cliente);
        }
        public async Task DeleteCliente(string id)
        {
            await _clienteRepository.DeleteCliente(id);
        }

    }
}
