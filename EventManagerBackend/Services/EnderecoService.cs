using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;

namespace EventManagerBackend.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }
        public async Task<IEnumerable<Endereco>> GetAllEnderecos()
        {
            return await _enderecoRepository.GetAllEnderecos();
        }
        public async Task<Endereco?> GetEnderecoById(int id)
        {
            return await _enderecoRepository.GetEnderecoById(id);
        }
        public async Task AddEndereco(Endereco endereco)
        {
            //Validação campos obrigatórios
            if (endereco.CEP <= 0)
            {
                throw new ArgumentException("O CEP é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(endereco.Rua))
            {
                throw new ArgumentException("O nome da rua é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(endereco.Bairro))
            {
                throw new ArgumentException("O nome do bairro é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(endereco.Cidade))
            {
                throw new ArgumentException("O nome da cidade é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(endereco.UF))
            {
                throw new ArgumentException("O UF do estado é obrigatório.");
            }

            await _enderecoRepository.AddEndereco(endereco);
        }
        public async Task UpdateEndereco(int id, UpdateEnderecoDTO dto)
        {
            //Validação campos obrigatórios
            if (id <= 0)
            {
                throw new ArgumentException("ID do endereço é obrigatório.");
            }
            //Verifica se o endereço existe na base
            var existingEndereco = await _enderecoRepository.GetEnderecoById(id);
            if (existingEndereco == null)
            {
                throw new ArgumentException("Endereço não encontrado.");
            }
            if (dto.CEP > 0) 
            {
                existingEndereco.CEP = dto.CEP;
            }
            if (!string.IsNullOrWhiteSpace(dto.Rua))
            {
                existingEndereco.Rua = dto.Rua;
            }
            if (dto.Numero != null & dto.Numero > 0)
            {
                existingEndereco.Numero = dto.Numero;
            }
            if (!string.IsNullOrWhiteSpace(dto.Complemento))
            {
                existingEndereco.Complemento = dto.Complemento;
            }          
            if (!string.IsNullOrWhiteSpace(dto.Bairro))
            {
                existingEndereco.Bairro = dto.Bairro;
            }
            if (!string.IsNullOrWhiteSpace(dto.Cidade))
            {
                existingEndereco.Cidade = dto.Cidade;
            }
            if (!string.IsNullOrWhiteSpace(dto.UF))
            {
                existingEndereco.UF = dto.UF;
            }
            await _enderecoRepository.UpdateEndereco(existingEndereco);
        }
        public async Task DeleteEndereco(int id)
        {
            var existingEndereco = await _enderecoRepository.GetEnderecoById(id);
            if (existingEndereco == null)
            {
                throw new ArgumentException("Endereço não encontrado.");
            }
            await _enderecoRepository.DeleteEndereco(id);
        }        
    }
}
