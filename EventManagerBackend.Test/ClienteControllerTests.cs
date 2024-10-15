using EventManagerBackend.Controllers;
using EventManagerBackend.DTOs;
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
            var usuario = "usuarioTeste";
            var cliente = new UpdateClienteDTO { Nome = "Fulano de tal" };
            _mockClienteService.Setup(s => s.UpdateCliente(usuario, cliente)).Returns(Task.CompletedTask);

            var result = await _clienteController.UpdateCliente(usuario, cliente);

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

        [Test]
        public async Task Login_ShouldReturnOKResult_WhenClienteIsValid_TesteSucesso()
        {
            var loginDTO = new LoginDTO { Usuario = "usuarioTeste", Senha = "xpto" };

            var cliente = new Cliente { Usuario = "usuarioTeste", Senha = "xpto", Nome = "Fulano de tal" };

            _mockClienteService.Setup(s => s.GetClienteByUsuario(loginDTO.Usuario)).ReturnsAsync(cliente);

            var result = await _clienteController.Login(loginDTO);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
            var clienteRetornado = okResult.Value as Cliente;
            Assert.IsNotNull(clienteRetornado);
            Assert.AreEqual(cliente.Usuario, clienteRetornado.Usuario);
            Assert.AreEqual(cliente.Nome, clienteRetornado.Nome);
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
        public async Task UpdateCliente_ShouldReturnBadRequest_WhenEmailInvalido_TesteFalha()
        {
            _clienteController.ModelState.AddModelError("Email", "O formato de e-mail é inválido.");
            var usuario = "usuarioTeste";
            var cliente = new UpdateClienteDTO { Email = "Fulano de Tal" };

            var result = await _clienteController.UpdateCliente(usuario, cliente);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Login_ShouldReturnNotFound_WhenCredentialsInvalid_TesteFalha()
        {
            var loginDTO = new LoginDTO { Usuario = "usuarioTeste", Senha = "xpto" };

            _mockClienteService.Setup(s => s.GetClienteByUsuario(loginDTO.Usuario)).ReturnsAsync((Cliente)null);

            var result = await _clienteController.Login(loginDTO);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Usuário ou senha incorretos.", notFoundResult.Value);
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenSenhaIsIncorrect_TesteFalha()
        {
            var loginDTO = new LoginDTO { Usuario = "usuarioTeste", Senha = "senhaErrada" };
            var cliente = new Cliente { Usuario = "usuarioTeste", Senha = "senhaCorreta", Nome = "Fulano de tal" };

            _mockClienteService.Setup(s => s.GetClienteByUsuario(loginDTO.Usuario)).ReturnsAsync(cliente);

            var result = await _clienteController.Login(loginDTO);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult?.StatusCode);
            Assert.AreEqual("Senha incorreta.", badRequestResult?.Value);
        }
        #endregion
    }
}
