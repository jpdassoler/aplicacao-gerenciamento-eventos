using EventManagerBackend.Controllers;
using EventManagerBackend.Models;
using EventManagerBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class ClienteControllerTests
    {
        private Mock<IClienteService> _mockClienteService;
        private ClienteController _clienteController;

        [SetUp]
        public void Setup()
        {
            _mockClienteService = new Mock<IClienteService>();
            _clienteController = new ClienteController(_mockClienteService.Object);
        }

        #region Testes de sucesso
        [Test]
        public async Task GetAllClientes_ShouldReturnOkResult_WithClienteList_TesteSucesso()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { Usuario = "usuarioTeste1", Nome = "Fulano de tal", Senha = "xpto" },
                new Cliente { Usuario = "usuarioTeste2", Nome = "Fulano de tal 2", Senha = "xpto" },
            };
            _mockClienteService.Setup(s => s.GetAllClientes()).ReturnsAsync(clientes);

            var result = await _clienteController.GetAllClientes();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(clientes, okResult.Value);
        }

        [Test]
        public async Task GetClienteByUsuario_ShouldReturnOkResult_WithCliente_WhenClienteExists_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de tal", Senha = "xpto" };
            _mockClienteService.Setup(s => s.GetClienteByUsuario(cliente.Usuario)).ReturnsAsync(cliente);

            var result = await _clienteController.GetClienteByUsuario(cliente.Usuario);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.AreEqual(cliente, okResult.Value);
        }

        [Test]
        public async Task AddCliente_ShouldReturnCreatedAtActionResult_WhenClienteIsValid_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de tal", Senha = "xpto" };
            _mockClienteService.Setup(s => s.AddCliente(cliente)).Returns(Task.CompletedTask);

            var result = await _clienteController.AddCliente(cliente);

            var createdAtResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            Assert.AreEqual(201, createdAtResult.StatusCode);
            Assert.AreEqual(cliente, createdAtResult.Value);
        }

        [Test]
        public async Task UpdateCliente_ShouldReturnNoContent_WhenClienteIsValid_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de tal" };
            _mockClienteService.Setup(s => s.UpdateCliente(cliente)).Returns(Task.CompletedTask);

            var result = await _clienteController.UpdateCliente(cliente.Usuario, cliente);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteCliente_ShouldReturnNoContent_WhenClienteIsValid_TesteSucesso()
        {
            var usuario = "usuarioTeste";
            _mockClienteService.Setup(s => s.DeleteCliente(usuario)).Returns(Task.CompletedTask);

            var result = await _clienteController.DeleteCliente(usuario);

            Assert.IsInstanceOf<NoContentResult>(result);
        }
        #endregion

        #region Teste de falha
        [Test]
        public async Task GetClienteByUsuario_ShouldReturnNotFound_WhenClienteDoesNotExist_TesteFalha()
        {
            var usuario = "usuarioTeste";
            _mockClienteService.Setup(s => s.GetClienteByUsuario(usuario)).ReturnsAsync((Cliente)null);

            var result = await _clienteController.GetClienteByUsuario(usuario);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddCliente_ShouldReturnBadRequest_WhenClienteNotInformed_TesteFalha()
        {
            var result = await _clienteController.AddCliente(null);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddCliente_ShouldReturnBadRequest_WhenUsuarioNotInformed_TesteFalha()
        {
            _clienteController.ModelState.AddModelError("Usuario", "Required");
            var result = await _clienteController.AddCliente(new Cliente());

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateCliente_ShouldReturnBadRequest_WhenUsuarioDoesNotMatch_TesteFalha()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste1", Nome = "Fulano de Tal" };

            var result = await _clienteController.UpdateCliente("usuarioTeste2", cliente);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateCliente_ShouldReturnBadRequest_WhenEmailInvalido_TesteFalha()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Email = "email" };
            _clienteController.ModelState.AddModelError("Email", "O formato de e-mail é inválido.");
            var result = await _clienteController.UpdateCliente(cliente.Usuario, cliente);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        #endregion
    }
}
