using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;
using EventManagerBackend.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class EnderecoServiceTests
    {
        private Mock<IEnderecoRepository> _mockEnderecoRepository;
        private EnderecoService _enderecoService;

        [SetUp]
        public void Setup()
        {
            _mockEnderecoRepository = new Mock<IEnderecoRepository>();
            _enderecoService = new EnderecoService(_mockEnderecoRepository.Object);
        }

        #region Testes de sucesso
        [Test]
        public async Task GetAllEnderecos_ShouldReturnAllEnderecos_TesteSucesso()
        {
            var enderecos = new List<Endereco>
            {
                new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" },
                new Endereco { ID_Endereco = 2, CEP = 87654321, Rua = "Rua de tal 2", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" }
            };

            _mockEnderecoRepository.Setup(repo => repo.GetAllEnderecos()).ReturnsAsync(enderecos);

            var result = await _enderecoService.GetAllEnderecos();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(e => e.ID_Endereco == 1));
            Assert.IsTrue(result.Any(e => e.ID_Endereco == 2));
        }

        [Test]
        public async Task GetEnderecoById_ShouldReturnEndereco_WhenEnderecoExists_TesteSucesso()
        {
            var endereco = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(endereco.ID_Endereco)).ReturnsAsync(endereco);

            var result = await _enderecoService.GetEnderecoById(endereco.ID_Endereco);

            Assert.IsNotNull(result);
            Assert.AreEqual(endereco, result);
        }

        [Test]
        public async Task AddEndereco_ShouldAddEndereco_WhenEnderecoIsValid_TesteSucesso()
        {
            var endereco = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            await _enderecoService.AddEndereco(endereco);

            _mockEnderecoRepository.Verify(repo => repo.AddEndereco(endereco), Times.Once);
        }

        [Test]
        public async Task UpdateEndereco_ShouldUpdateEndereco_WhenEnderecoExists_TesteSucesso()
        {
            var enderedoExistente = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            var dto = new UpdateEnderecoDTO { Rua = "Rua Alterada", CEP = 87654321, Bairro = "Bairro alterado", Cidade = "São Paulo", UF = "SP", Numero = 123, Complemento = "apto" };

            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(enderedoExistente.ID_Endereco)).ReturnsAsync(enderedoExistente);

            await _enderecoService.UpdateEndereco(enderedoExistente.ID_Endereco, dto);

            _mockEnderecoRepository.Verify(repo => repo.UpdateEndereco(It.Is<Endereco>(e => e.Rua == "Rua Alterada" && e.ID_Endereco == enderedoExistente.ID_Endereco)), Times.Once);
        }

        [Test]
        public async Task DeleteEndereco_ShouldDeleteEndereco_WhenEnderecoExists_TesteSucesso()
        {
            var enderedoExistente = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(enderedoExistente.ID_Endereco)).ReturnsAsync(enderedoExistente);

            await _enderecoService.DeleteEndereco(enderedoExistente.ID_Endereco);

            _mockEnderecoRepository.Verify(repo => repo.DeleteEndereco(enderedoExistente.ID_Endereco), Times.Once);
        }
        #endregion

        #region Testes de falha
        [Test]
        public void AddEndereco_ShouldThrowArgumentException_WhenCEPNaoInformado()
        {
            var endereco = new Endereco { Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.AddEndereco(endereco));
            Assert.AreEqual("O CEP é obrigatório.", ex.Message);
        }

        [Test]
        public void AddEndereco_ShouldThrowArgumentException_WhenRuaNaoInformado()
        {
            var endereco = new Endereco { CEP = 12345678, Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.AddEndereco(endereco));
            Assert.AreEqual("O nome da rua é obrigatório.", ex.Message);
        }

        [Test]
        public void AddEndereco_ShouldThrowArgumentException_WhenBairroNaoInformado()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de tal", Cidade = "Porto Alegre", UF = "RS" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.AddEndereco(endereco));
            Assert.AreEqual("O nome do bairro é obrigatório.", ex.Message);
        }

        [Test]
        public void AddEndereco_ShouldThrowArgumentException_WhenCidadeNaoInformado()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", UF = "RS" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.AddEndereco(endereco));
            Assert.AreEqual("O nome da cidade é obrigatório.", ex.Message);
        }

        [Test]
        public void AddEndereco_ShouldThrowArgumentException_WhenUFNaoInformado()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.AddEndereco(endereco));
            Assert.AreEqual("O UF do estado é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateEndereco_ShouldThrownArgumentException_WhenIDNaoInformado()
        {
            var dto = new UpdateEnderecoDTO { CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.UpdateEndereco(0, dto));
            Assert.AreEqual("ID do endereço é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateEndereco_ShouldThrownArgumentException_WhenEnderecoDoesNotExist()
        {
            var dto = new UpdateEnderecoDTO { CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(99)).ReturnsAsync((Endereco)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.UpdateEndereco(99, dto));
            Assert.AreEqual("Endereço não encontrado.", ex.Message);
        }

        [Test]
        public void DeleteEndereco_ShouldThrowArgumentException_WhenEnderecoDosNotExist()
        {
            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(99)).ReturnsAsync((Endereco)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _enderecoService.DeleteEndereco(99));
            Assert.AreEqual("Endereço não encontrado.", ex.Message);
        }

        [Test]
        public async Task GetAllEnderecos_ShouldReturnNull_WhenEnderecoDoesNotExist()
        {
            _mockEnderecoRepository.Setup(repo => repo.GetAllEnderecos()).ReturnsAsync(Enumerable.Empty<Endereco>().AsQueryable());
            var result = await _enderecoService.GetAllEnderecos();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetEnderecoByID_ShouldReturnNull_WhenEnderecoDoesNotExist()
        {
            _mockEnderecoRepository.Setup(repo => repo.GetEnderecoById(99)).ReturnsAsync((Endereco)null);

            var result = await _enderecoService.GetEnderecoById(99);

            Assert.IsNull(result);
        }
        #endregion
    }
}
