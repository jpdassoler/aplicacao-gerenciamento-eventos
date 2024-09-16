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
        public async Task<Cliente> GetClienteByUsuario(string usuario)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Usuario == usuario);
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
        public async Task DeleteCliente(string usuario)
        {
            var cliente = await GetClienteByUsuario(usuario);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

    }
}
