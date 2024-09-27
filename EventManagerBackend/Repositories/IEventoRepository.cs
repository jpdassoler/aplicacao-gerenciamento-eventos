using EventManagerBackend.Models;

namespace EventManagerBackend.Repositories
{
    public interface IEventoRepository
    {
        Task<IEnumerable<Evento>> GetAllEventos();
        Task<Evento?> GetEventoById(int id);
        Task AddEvento(Evento evento);
        Task UpdateEvento(Evento evento);
        Task DeleteEvento(int id);
    }
}
