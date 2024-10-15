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
    public class ClienteEventoControllerTests
    {
        private Mock<IClienteEventoService> _mockClienteEventoService;
        private ClienteEventoController _clienteEventoController;

        [SetUp]
        public void SetUp()
        {
            _mockClienteEventoService = new Mock<IClienteEventoService>();
            _clienteEventoController = new ClienteEventoController(_mockClienteEventoService.Object);
        }

        #region Testes de Sucesso
        [Test]
        public async Task GetAllClienteEventos_ShouldReturnOkResult_WithClienteEventoList_TesteSucesso()
        {
            var clienteEventos = new List<ClienteEvento>
            {
                new ClienteEvento { Usuario = "usuarioTeste1", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim },
                new ClienteEvento { Usuario = "usuarioTeste2", ID_Evento = 2, Ind_Comparecimento = EnumIndComparecimento.Organizador }
            };

            _mockClienteEventoService.Setup(service => service.GetAllClienteEventos()).ReturnsAsync(clienteEventos);

            var result = await _clienteEventoController.GetAllClienteEventos();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(clienteEventos, okResult.Value);
        }

        [Test]
        public async Task GetClienteEventoById_ShouldReturnOkResult_WithClienteEvento_WhenClienteEventoExists_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            _mockClienteEventoService.Setup(service => service.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento)).ReturnsAsync(clienteEvento);

            var result = await _clienteEventoController.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(clienteEvento, okResult.Value);
        }

        [Test]
        public async Task AddClienteEvento_ShouldReturnCreatedAtActionResult_WhenClienteEventoIsValid_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            _mockClienteEventoService.Setup(service => service.AddClienteEvento(clienteEvento)).Returns(Task.CompletedTask);

            var result = await _clienteEventoController.AddClienteEvento(clienteEvento);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(clienteEvento, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateClienteEvento_ShouldReturnNoContent_WhenClienteEventoIsUpdated_TesteSucesso()
        {
            var usuario = "usuarioTeste";
            var idEvento = 1;
            var dto = new UpdateClienteEventoDTO { Ind_Comparecimento = EnumIndComparecimento.Sim };

            _mockClienteEventoService.Setup(service => service.UpdateClienteEvento(usuario, idEvento, dto)).Returns(Task.CompletedTask);

            var result = await _clienteEventoController.UpdateClienteEvento(usuario, idEvento, dto);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteClienteEvento_ShouldReturnNoContent_WhenClienteEventoIsDeleted_TesteSucesso()
        {
            var usuario = "usuarioTeste";
            var idEvento = 1;
            _mockClienteEventoService.Setup(service => service.DeleteClienteEvento(usuario, idEvento)).Returns(Task.CompletedTask);

            var result = await _clienteEventoController.DeleteClienteEvento(usuario, idEvento);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
        #endregion

        #region Testes de Falha
        [Test]
        public async Task GetClienteEventoById_ShouldReturnNotFound_WhenClienteEventoDoesNotExist_TesteFalha()
        {
            var usuario = "usuarioFalha";
            var idEvento = 99;
            _mockClienteEventoService.Setup(service => service.GetClienteEventoById(usuario, idEvento)).ReturnsAsync((ClienteEvento)null);

            var result = await _clienteEventoController.GetClienteEventoById(usuario, idEvento);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddClienteEvento_ShouldReturnBadRequest_WhenClienteEventoNotInformed_TesteFalha()
        {
            var result = await _clienteEventoController.AddClienteEvento(null);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task GetClienteEventoByEventoAndComparecimento_ShouldReturnOkResult_WithClientesList_WhenParametersValid_TesteSucesso()
        {
            int idEvento = 1;
            char indComparecimento = 'S';

            var clientesRetornados = new List<ClienteEventoDetalhesDTO>
            {
            new ClienteEventoDetalhesDTO { Usuario = "usuario1", Nome = "Cliente 1", IndComparecimento = EnumIndComparecimento.Sim },
            new ClienteEventoDetalhesDTO { Usuario = "usuario2", Nome = "Cliente 2", IndComparecimento = EnumIndComparecimento.Sim }
            };

            _mockClienteEventoService.Setup(s => s.GetClientesByEventoAndComparecimento(idEvento, EnumIndComparecimento.Sim))
                .ReturnsAsync(clientesRetornados);

            var result = await _clienteEventoController.GetClientesByEventoAndComparecimento(idEvento, indComparecimento);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(clientesRetornados, okResult.Value);
        }
    
        #endregion
    }
}
