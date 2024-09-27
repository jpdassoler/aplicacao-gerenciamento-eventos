using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;

namespace EventManagerBackend.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;
        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }
        public async Task<IEnumerable<Evento>> GetAllEventos()
        {
            return await _eventoRepository.GetAllEventos();
        }
        public async Task<Evento?> GetEventoById(int id)
        {
            return await _eventoRepository.GetEventoById(id);
        }
        public async Task AddEvento(Evento evento)
        {
            //Validação campos obrigatórios
            if (evento.ID_Evento <= 0)
            {
                throw new ArgumentException("O evento precisa ter um endereço cadastrado.");
            }
            if (String.IsNullOrWhiteSpace(evento.Nome))
            {
                throw new ArgumentException("O nome do evento é obrigatório.");
            }
            if (evento.Data == default(DateTime))
            {
                throw new ArgumentException("A data do evento é obrigatória.");
            }
            if (evento.Data < DateTime.Now)
            {
                throw new ArgumentException("A data do evento não pode ser no passado.");
            }
            await _eventoRepository.AddEvento(evento);
        }
        public async Task UpdateEvento(int id, UpdateEventoDTO dto) 
        {
            //Validação campos obrigatórios
            if (id <= 0)
            {
                throw new ArgumentException("ID do evento é obrigatório.");
            }
            //Verifica se o endereço existe na base
            var existingEvento = await _eventoRepository.GetEventoById(id);
            if (existingEvento == null) 
            {
                throw new ArgumentException("Evento não encontrado.");
            }
            if (dto.ID_Endereco.HasValue & dto.ID_Endereco > 0)
            {
                existingEvento.ID_Endereco = dto.ID_Endereco.Value;
            }
            if (!string.IsNullOrWhiteSpace(dto.Nome))
            {
                existingEvento.Nome = dto.Nome;
            }
            if (dto.Data.HasValue)
            {
                if (dto.Data.Value < DateTime.Now)
                {
                    throw new ArgumentException("A data do evento não pode ser no passado.");
                }
                existingEvento.Data = dto.Data.Value;
            }
            if (!string.IsNullOrWhiteSpace(dto.Banner))
            {
                existingEvento.Banner = dto.Banner;
            }
            if (!string.IsNullOrWhiteSpace(dto.Descricao))
            {
                existingEvento.Descricao = dto.Descricao;
            }
            if (dto.PrecoIngresso.HasValue)
            {
                if (dto.PrecoIngresso.Value < 0)
                {
                    throw new ArgumentException("O preço do ingresso deve ser positivo.");
                }
                existingEvento.PrecoIngresso = dto.PrecoIngresso.Value;
            }
            if (!string.IsNullOrWhiteSpace(dto.URLIngresso))
            {
                existingEvento.URLIngresso = dto.URLIngresso;
            }
            await _eventoRepository.UpdateEvento(existingEvento);
        }
        public async Task DeleteEvento(int id)
        {
            var evento = await _eventoRepository.GetEventoById(id);
            if (evento == null) 
            {
                throw new ArgumentException("Evento não encontrado.");
            }
            await _eventoRepository.DeleteEvento(id);
        }
    }
}
