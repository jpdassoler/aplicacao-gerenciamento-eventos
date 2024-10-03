using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;

namespace EventManagerBackend.Services
{
    public class ClienteEventoService : IClienteEventoService
    {
        private readonly IClienteEventoRepository _clienteEventoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEventoRepository _eventoRepository;
        public ClienteEventoService(IClienteEventoRepository clienteEventoRepository, IClienteRepository clienteRepository, IEventoRepository eventoRepository)
        {
            _clienteEventoRepository = clienteEventoRepository;
            _clienteRepository = clienteRepository;
            _eventoRepository = eventoRepository;
        }
        public async Task<IEnumerable<ClienteEvento>> GetAllClienteEventos()
        {
            return await _clienteEventoRepository.GetAllClienteEventos();
        }
        public async Task<ClienteEvento?> GetClienteEventoById(string usuario, int idEvento)
        {
            return await _clienteEventoRepository.GetClienteEventoById(usuario, idEvento);
        }
        public async Task AddClienteEvento(ClienteEvento clienteEvento)
        {
            //Validação dos campos obrigatórios
            if (clienteEvento.ID_Evento <= 0)
            {
                throw new ArgumentException("ID do evento é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(clienteEvento.Usuario))
            {
                throw new ArgumentException("Usuario é obrigatório.");
            }
            if (!Enum.IsDefined(typeof(EnumIndComparecimento), clienteEvento.Ind_Comparecimento))
            {
                throw new ArgumentException("O comparecimento deve ser Sim, Não, Talvez ou Organizador.");
            }
            //Validação de Evento existente
            var existingEvento = await _eventoRepository.GetEventoById(clienteEvento.ID_Evento);
            if (existingEvento == null)
            {
                throw new ArgumentException("Evento associado não encontrado.");
            }
            //Validação de Cliente existente
            var existingCliente = await _clienteRepository.GetClienteByUsuario(clienteEvento.Usuario);
            if (existingCliente == null)
            {
                throw new ArgumentException("Cliente associado não encontrado.");
            }
            await _clienteEventoRepository.AddClienteEvento(clienteEvento);
        }
        public async Task UpdateClienteEvento(string usuario, int idEvento, UpdateClienteEventoDTO dto)
        {
            //Validação campos obrigatórios
            if (idEvento <= 0)
            {
                throw new ArgumentException("ID do evento é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentException("Usuario é obrigatório.");
            }
            //Verifica se o Cliente_Evento existe na base
            var existingClienteEvento = await _clienteEventoRepository.GetClienteEventoById(usuario, idEvento);
            if (existingClienteEvento == null) 
            {
                throw new ArgumentException("Registro de Cliente_Evento não encontrado.");
            }

            if (dto.Ind_Comparecimento.HasValue)
            {
                if (!Enum.IsDefined(typeof(EnumIndComparecimento), dto.Ind_Comparecimento))
                {
                    throw new ArgumentException("O comparecimento deve ser Sim, Não, Talvez ou Organizador.");
                }
                existingClienteEvento.Ind_Comparecimento = dto.Ind_Comparecimento.Value;
            }
            await _clienteEventoRepository.UpdateClienteEvento(existingClienteEvento);
        }
        public async Task DeleteClienteEvento(string usuario, int idEvento)
        {
            var clienteEvento = await _clienteEventoRepository.GetClienteEventoById(usuario, idEvento);
            if (clienteEvento == null)
            {
                throw new ArgumentException("Cliente_Evento não encontrado.");
            }
            await _clienteEventoRepository.DeleteClienteEvento(usuario, idEvento);
        }
    }
}
