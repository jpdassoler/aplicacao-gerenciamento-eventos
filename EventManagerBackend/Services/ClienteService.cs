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
        public async Task<Cliente> GetClienteByUsuario(string usuario) 
        {
            return await _clienteRepository.GetClienteByUsuario(usuario);
        }
        public async Task AddCliente(Cliente cliente)
        {
            //Validação campos obrigatórios
            if (String.IsNullOrEmpty(cliente.Usuario))
            {
                throw new ArgumentException("Usuário é obrigatório.");
            }
            if (String.IsNullOrEmpty(cliente.Senha))
            {
                throw new ArgumentException("Senha é obrigatória.");
            }
            if (String.IsNullOrEmpty(cliente.Nome))
            {
                throw new ArgumentException("Nome é obrigatório.");
            }
            //Garantia de que o usuário é único na base
            var existingCliente = await _clienteRepository.GetClienteByUsuario(cliente.Usuario);
            if (existingCliente != null) 
            {
                throw new ArgumentException("Este nome de usuário já está em uso.");
            }
            await _clienteRepository.AddCliente(cliente);
        }
        public async Task UpdateCliente(Cliente cliente)
        {
            //Validação campos obrigatórios
            if (String.IsNullOrEmpty(cliente.Usuario))
            {
                throw new ArgumentException("Usuário é obrigatório.");
            }
            //Verifica se o usuário existe na base
            var existingCliente = await _clienteRepository.GetClienteByUsuario(cliente.Usuario);
            if (existingCliente == null)
            {
                throw new ArgumentException("Cliente não encontrado.");
            }
            //Verifica se há tentativa de alterar o campo Usuário
            if (existingCliente.Usuario != cliente.Usuario)
            {
                throw new ArgumentException("O nome de usuário não pode ser alterado.");
            }
            existingCliente.Senha = cliente.Senha;
            existingCliente.Telefone = cliente.Telefone;
            existingCliente.Nome = cliente.Nome;
            existingCliente.Instagram = cliente.Instagram;
            existingCliente.Email = cliente.Email;
            await _clienteRepository.UpdateCliente(existingCliente);
        }
        public async Task DeleteCliente(string usuario)
        {
            await _clienteRepository.DeleteCliente(usuario);
        }

    }
}
