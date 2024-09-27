using EventManagerBackend.DTOs;
using EventManagerBackend.Models;

namespace EventManagerBackend.Services
{
    public interface IEventoService
    {
        Task<IEnumerable<Evento>> GetAllEventos();
        Task<Evento?> GetEventoById(int id);
        Task AddEvento(Evento evento);
        Task UpdateEvento(int id, UpdateEventoDTO evento);
        Task DeleteEvento(int id);
    }
}
