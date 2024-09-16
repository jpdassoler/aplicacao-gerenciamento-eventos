using EventManagerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerBackend.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return await _context.Clientes.ToListAsync();
        }
        public async Task<Cliente> GetClienteById(string id)
        {
            return await _context.Clientes.FindAsync(id);
        }
        public async Task AddCliente(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCliente(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCliente(string id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
