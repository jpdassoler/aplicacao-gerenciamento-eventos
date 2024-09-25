using EventManagerBackend.DTOs;
using EventManagerBackend.Models;

namespace EventManagerBackend.Services
{
    public interface IEnderecoService
    {
        Task<IEnumerable<Endereco>> GetAllEnderecos();
        Task<Endereco?> GetEnderecoById(int id);
        Task AddEndereco(Endereco endereco);
        Task UpdateEndereco(int id, UpdateEnderecoDTO endereco);
        Task DeleteEndereco(int id);
    }
}
