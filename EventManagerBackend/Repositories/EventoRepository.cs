using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerBackend.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly AppDbContext _context;        
        public EventoRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Evento>> GetAllEventos()
        {
            return await _context.Eventos.Include(e => e.Endereco).ToListAsync();
        }
        public async Task<Evento?> GetEventoById(int id)
        {
            return await _context.Eventos.Include(e => e.Endereco).FirstOrDefaultAsync(e => e.ID_Evento == id);
        }
        public async Task AddEvento(Evento evento)
        {
            await _context.Eventos.AddAsync(evento);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEvento(Evento evento)
        {
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEvento(int id)
        {
            var evento = await GetEventoById(id);
            if (evento != null) 
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync(); 
            }
        }

    }
}
