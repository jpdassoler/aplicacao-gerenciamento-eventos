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
    public class EventoServiceTests
    {
        private Mock<IEventoRepository> _mockEventoRepository;
        private EventoService _eventoService;

        [SetUp]
        public void Setup()
        {
            _mockEventoRepository = new Mock<IEventoRepository>();
            _eventoService = new EventoService( _mockEventoRepository.Object );
        }

        #region Testes de sucesso
        [Test]
        public async Task GetAllEventos_ShouldReturnAllEventos_TesteSucesso()
        {
            var eventos = new List<Evento>
            {
                new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 },
                new Evento { ID_Evento = 2, Nome = "Evento 2", Data = DateTime.Now.AddDays(3), Preco_Ingresso = 150.0m, ID_Endereco = 1 }
            };

            _mockEventoRepository.Setup(repo => repo.GetAllEventos()).ReturnsAsync(eventos);

            var result = await _eventoService.GetAllEventos();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(e => e.ID_Evento == 1));
            Assert.IsTrue(result.Any(e => e.ID_Evento == 2));
        }

        [Test]
        public async Task GetEventoById_ShouldReturnEvento_WhenEventoExists_TesteSucesso()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };

            _mockEventoRepository.Setup(repo => repo.GetEventoById(evento.ID_Evento)).ReturnsAsync(evento);

            var result = await _eventoService.GetEventoById(evento.ID_Evento);

            Assert.IsNotNull(result);
            Assert.AreEqual(evento, result);
        }

        [Test]
        public async Task AddEvento_ShouldAddEvento_WhenEventoIsValid_TesteSucesso()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };

            await _eventoService.AddEvento(evento);

            _mockEventoRepository.Verify(repo => repo.AddEvento(evento), Times.Once);
        }

        [Test]
        public async Task UpdateEvento_ShouldUpdateEvento_WhenEventoExists_TesteSucesso()
        {
            var eventoExistente = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };

            var dto = new UpdateEventoDTO { Nome = "Nome alterado", Descricao = "Descrição alterada", Data = DateTime.Now.AddMonths(1), Banner = "https://banner",
                Preco_Ingresso = 150.0m, ID_Endereco = 3, URL_Ingresso = "https.//ingresso" };

            _mockEventoRepository.Setup(repo => repo.GetEventoById(eventoExistente.ID_Evento)).ReturnsAsync(eventoExistente);

            await _eventoService.UpdateEvento(eventoExistente.ID_Evento, dto);

            _mockEventoRepository.Verify(repo => repo.UpdateEvento(It.Is<Evento>(e => e.Nome == dto.Nome && e.Descricao == dto.Descricao)), Times.Once);
        }

        [Test]
        public async Task DeleteEvento_ShouldDeleteEvento_WhenEventoExists_TesteSucesso()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };

            _mockEventoRepository.Setup(repo => repo.GetEventoById(evento.ID_Evento)).ReturnsAsync(evento);

            await _eventoService.DeleteEvento(evento.ID_Evento);

            _mockEventoRepository.Verify(repo => repo.DeleteEvento(evento.ID_Evento), Times.Once);
        }
        #endregion

        #region Testes de falha
        [Test]
        public void AddEvento_ShouldThrowArgumentException_WhenNomeNaoInformado()
        {
            var evento = new Evento { ID_Endereco = 1, Data = DateTime.Now.AddDays(2) };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.AddEvento(evento));
            Assert.AreEqual("O nome do evento é obrigatório.", ex.Message);
        }

        [Test]
        public void AddEvento_ShouldThrowArgumentException_WhenDataNaoInformada()
        {
            var evento = new Evento { ID_Endereco = 1, Nome = "Evento 1" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.AddEvento(evento));
            Assert.AreEqual("A data do evento é obrigatória.", ex.Message);
        }

        [Test]
        public void AddEvento_ShouldThrowArgumentException_WhenDataNoPassado()
        {
            var evento = new Evento { ID_Endereco = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(-1) };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.AddEvento(evento));
            Assert.AreEqual("A data do evento não pode ser no passado.", ex.Message);
        }

        [Test]
        public void UpdateEvento_ShouldThrownArgumentException_WhenIDNaoInformado()
        {
            var dto = new UpdateEventoDTO { ID_Endereco = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(1) };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.UpdateEvento(0, dto));
            Assert.AreEqual("ID do evento é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateEvento_ShouldThrownArgumentException_WhenEventoDoesNotExist()
        {
            var dto = new UpdateEventoDTO { ID_Endereco = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(1) };
            _mockEventoRepository.Setup(repo => repo.GetEventoById(99)).ReturnsAsync((Evento)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.UpdateEvento(99, dto));
            Assert.AreEqual("Evento não encontrado.", ex.Message);
        }

        [Test]
        public void UpdateEvento_ShouldThrownArgumentException_WhenDataNoPassado()
        {
            var eventoExistente = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };
            var idEndereco = 1;
            var dto = new UpdateEventoDTO { ID_Endereco = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(-1) };
            _mockEventoRepository.Setup(repo => repo.GetEventoById(eventoExistente.ID_Evento)).ReturnsAsync(eventoExistente);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.UpdateEvento(idEndereco, dto));
            Assert.AreEqual("A data do evento não pode ser no passado.", ex.Message);
        }

        [Test]
        public void UpdateEvento_ShouldThrownArgumentException_WhenIngressoNegativo()
        {
            var eventoExistente = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 100.0m, ID_Endereco = 1 };
            var idEndereco = 1;
            var dto = new UpdateEventoDTO { ID_Endereco = 1, Nome = "Evento 1", Preco_Ingresso = -100 };
            _mockEventoRepository.Setup(repo => repo.GetEventoById(eventoExistente.ID_Evento)).ReturnsAsync(eventoExistente);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.UpdateEvento(idEndereco, dto));
            Assert.AreEqual("O preço do ingresso deve ser positivo.", ex.Message);
        }

        [Test]
        public void DeleteEvento_ShouldThrowArgumentException_WhenEventoDosNotExist()
        {
            _mockEventoRepository.Setup(repo => repo.GetEventoById(99)).ReturnsAsync((Evento)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _eventoService.DeleteEvento(99));
            Assert.AreEqual("Evento não encontrado.", ex.Message);
        }

        [Test]
        public async Task GetAlleventos_ShouldReturnNull_WhenEventoDoesNotExist()
        {
            _mockEventoRepository.Setup(repo => repo.GetAllEventos()).ReturnsAsync(Enumerable.Empty<Evento>().AsQueryable());
            var result = await _eventoService.GetAllEventos();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetEventoByID_ShouldReturnNull_WhenEventoDoesNotExist()
        {
            _mockEventoRepository.Setup(repo => repo.GetEventoById(99)).ReturnsAsync((Evento)null);

            var result = await _eventoService.GetEventoById(99);

            Assert.IsNull(result);
        }
        #endregion
    }
}
