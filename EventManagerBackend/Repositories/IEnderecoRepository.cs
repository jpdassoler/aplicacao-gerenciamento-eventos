using EventManagerBackend.Models;

namespace EventManagerBackend.Repositories
{
    public interface IEnderecoRepository
    {
        Task<IEnumerable<Endereco>> GetAllEnderecos();
        Task<Endereco?> GetEnderecoById(int id);
        Task AddEndereco(Endereco endereco);
        Task UpdateEndereco(Endereco endereco);
        Task DeleteEndereco(int id);
    }
}
