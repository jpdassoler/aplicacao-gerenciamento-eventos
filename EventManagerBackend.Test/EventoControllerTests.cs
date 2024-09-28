using EventManagerBackend.Controllers;
using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class EventoControllerTests
    {
        private Mock<IEventoService> _mockEventoService;
        private EventoController _eventoController;

        [SetUp]
        public void SetUp()
        {
            _mockEventoService = new Mock<IEventoService>();
            _eventoController = new EventoController(_mockEventoService.Object);
        }

        #region Testes de sucesso
        [Test]
        public async Task GetAllEventos_ShouldReturnOkResult_WithEventosList_TesteSucesso()
        {
            var eventos = new List<Evento>
        {
            new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(1), Preco_Ingresso = 100.0m, ID_Endereco = 1 },
            new Evento { ID_Evento = 2, Nome = "Evento 2", Data = DateTime.Now.AddDays(2), Preco_Ingresso = 150.0m, ID_Endereco = 1 }
        };

            _mockEventoService.Setup(service => service.GetAllEventos()).ReturnsAsync(eventos);

            var result = await _eventoController.GetAllEventos();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(eventos, okResult.Value);
        }

        [Test]
        public async Task GetEventoById_ShouldReturnOk_WhenEventoExists_TesteSucesso()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(1), Preco_Ingresso = 100.0m, ID_Endereco = 1 };
            _mockEventoService.Setup(service => service.GetEventoById(evento.ID_Evento)).ReturnsAsync(evento);

            var result = await _eventoController.GetEventoById(evento.ID_Evento);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(evento, okResult.Value);
        }

        [Test]
        public async Task AddEvento_ShouldReturnCreatedAtActionResult_WhenEventoIsValid_TesteSucesso()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "Evento 1", Data = DateTime.Now.AddDays(1), Preco_Ingresso = 100.0m , ID_Endereco = 1 };

            _mockEventoService.Setup(service => service.AddEvento(evento)).Returns(Task.CompletedTask);

            var result = await _eventoController.AddEvento(evento);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(evento, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateEvento_ShouldReturnNoContent_WhenEventoIsUpdated_TesteSucesso()
        {
            var idEvento = 1;
            var dto = new UpdateEventoDTO { Nome = "Nome alterado", Descricao = "Descrição Alterada" };

            _mockEventoService.Setup(service => service.UpdateEvento(idEvento, dto)).Returns(Task.CompletedTask);

            var result = await _eventoController.UpdateEvento(idEvento, dto);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteEvento_ShouldReturnNoContent_WhenEventoIsDeleted_TesteSucesso()
        {
            var idEvento = 1;
            _mockEventoService.Setup(service => service.DeleteEvento(idEvento)).Returns(Task.CompletedTask);

            var result = await _eventoController.DeleteEvento(idEvento);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
        #endregion

        #region Testes de falha
        [Test]
        public async Task GetEventoById_ShouldReturnNotFound_WhenEventoDoesNotExist_TesteFalha()
        {
            var idEvento = 99;
            _mockEventoService.Setup(service => service.GetEventoById(idEvento)).ReturnsAsync((Evento)null);

            var result = await _eventoController.GetEventoById(idEvento);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddEvento_ShouldReturnBadRequest_WhenEventoNotInformed_TesteFalha()
        {
            var result = await _eventoController.AddEvento(null);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task AddEvento_ShouldReturnBadRequest_WhenNomeInvalido_TesteFalha()
        {
            var evento = new Evento { ID_Evento = 1, Nome = "123456789012345678901234567890123456789012345678901", Data = DateTime.Now.AddDays(1), 
                Preco_Ingresso = 100.0m, ID_Endereco = 1 };
            _eventoController.ModelState.AddModelError("Nome", "O nome do evento não pode ter mais de 50 caracteres.");
            var result = await _eventoController.AddEvento(evento);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateEvento_ShouldReturnBadRequest_WhenPrecoIngressoInvalido_TesteFalha()
        {
            _eventoController.ModelState.AddModelError("Preco_Ingresso", "O preço do ingresso deve ser positivo.");
            var idEvento = 1;
            var dto = new UpdateEventoDTO { Preco_Ingresso = -100 };

            var result = await _eventoController.UpdateEvento(idEvento, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateEvento_ShouldReturnNotFound_WhenEventoDoesNotExist_TesteFalha()
        {
            var idEvento = 99;
            var dto = new UpdateEventoDTO { Nome = "Nome Alterada" };

            _mockEventoService.Setup(service => service.UpdateEvento(idEvento, dto)).ThrowsAsync(new ArgumentException("Evento não encontrado."));

            var result = await _eventoController.UpdateEvento(idEvento, dto);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Evento não encontrado.", notFoundResult.Value);
        }

        [Test]
        public async Task DeleteEvento_ShouldReturnNotFound_WhenEventoDoesNotExist_TesteFalha()
        {
            var idEvento = 99;
            _mockEventoService.Setup(service => service.DeleteEvento(idEvento)).ThrowsAsync(new ArgumentException("Evento não encontrado."));

            var result = await _eventoController.DeleteEvento(idEvento);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Evento não encontrado.", notFoundResult.Value);
        }
        #endregion

    }
}
