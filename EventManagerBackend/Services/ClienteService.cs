using EventManagerBackend.DTOs;
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
        public async Task UpdateCliente(string usuario, UpdateClienteDTO dto)
        {
            //Validação campos obrigatórios
            if (String.IsNullOrEmpty(usuario))
            {
                throw new ArgumentException("Usuário é obrigatório.");
            }
            //Verifica se o usuário existe na base
            var existingCliente = await _clienteRepository.GetClienteByUsuario(usuario);
            if (existingCliente == null)
            {
                throw new ArgumentException("Cliente não encontrado.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                existingCliente.Senha = dto.Senha;
            }
            if (!string.IsNullOrWhiteSpace(dto.Telefone))
            {
                existingCliente.Telefone = dto.Telefone;
            }
            if (!string.IsNullOrWhiteSpace(dto.Nome))
            {
                existingCliente.Nome = dto.Nome;
            }
            if (!string.IsNullOrWhiteSpace(dto.Instagram))
            {
                existingCliente.Instagram = dto.Instagram;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                existingCliente.Email = dto.Email;
            }           
            await _clienteRepository.UpdateCliente(existingCliente);
        }
        public async Task DeleteCliente(string usuario)
        {
            var cliente = await _clienteRepository.GetClienteByUsuario(usuario);
            if (cliente == null)
            {
                throw new ArgumentException("Cliente não encontrado.");
            }
            await _clienteRepository.DeleteCliente(usuario);
        }

    }
}
