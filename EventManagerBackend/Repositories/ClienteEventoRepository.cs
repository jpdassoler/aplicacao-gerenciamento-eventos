using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerBackend.Repositories
{
    public class ClienteEventoRepository : IClienteEventoRepository
    {
        private readonly AppDbContext _context;
        public ClienteEventoRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<ClienteEvento>> GetAllClienteEventos()
        {
            return await _context.ClienteEventos.ToListAsync();
        }
        public async Task<ClienteEvento?> GetClienteEventoById(string usuario, int idEvento)
        {
            return await _context.ClienteEventos.FindAsync(usuario, idEvento);
        }
        public async Task AddClienteEvento(ClienteEvento clienteEvento)
        {
            await _context.ClienteEventos.AddAsync(clienteEvento);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateClienteEvento(ClienteEvento clienteEvento)
        {
            _context.ClienteEventos.Update(clienteEvento);
            _context.SaveChanges();
        }
        public async Task DeleteClienteEvento(string usuario, int idEvento)
        {
            var clienteEvento = await GetClienteEventoById(usuario, idEvento);
            if (clienteEvento != null) 
            {
                _context.ClienteEventos.Remove(clienteEvento);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ClienteEventoDetalhesDTO>> GetClientesByEventoAndComparecimento(int idEvento, EnumIndComparecimento indComparecimento)
        {
            var result = await (from ce in _context.ClienteEventos
                                join c in _context.Clientes
                                on ce.Usuario equals c.Usuario
                                where ce.ID_Evento == idEvento && ce.Ind_Comparecimento == indComparecimento
                                select new ClienteEventoDetalhesDTO
                                {
                                    Usuario = ce.Usuario,
                                    Nome = c.Nome,
                                    IndComparecimento = ce.Ind_Comparecimento
                                }).ToListAsync();
            return result;
        }
    }
}
