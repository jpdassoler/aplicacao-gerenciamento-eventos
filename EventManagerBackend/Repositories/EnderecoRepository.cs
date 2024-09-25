using EventManagerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerBackend.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly AppDbContext _context;

        public EnderecoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Endereco>> GetAllEnderecos()
        {
            return await _context.Enderecos.ToListAsync();
        }
        public async Task<Endereco?> GetEnderecoById(int id)
        {
            return await _context.Enderecos.FindAsync(id);
        }
        public async Task AddEndereco(Endereco endereco)
        {
            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEndereco(Endereco endereco)
        {
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEndereco(int id)
        {
            var endereco = await GetEnderecoById(id);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
        }
    }
}
