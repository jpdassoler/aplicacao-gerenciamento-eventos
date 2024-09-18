using EventManagerBackend.Models;
using EventManagerBackend.Repositories;
using EventManagerBackend.Services;
using Moq;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class ClienteServiceTests
    {
        private Mock<IClienteRepository> _mockClienteRepository;
        private ClienteService _clienteService;

        [SetUp]
        public void Setup()
        {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_mockClienteRepository.Object);
        }

        #region Testes Sucesso
        [Test]
        public async Task GetAllClientes_ReturnsAllClientes_TesteSucesso()
        {
            var clientes = new List<Cliente> 
            {
                new Cliente { Usuario = "usuarioTeste1", Nome = "Teste1", Senha = "xpto" },
                new Cliente { Usuario = "usuarioTeste2", Nome = "Teste2", Senha = "xpto" }
            }.AsQueryable();

            _mockClienteRepository.Setup(repo => repo.GetAllClientes()).ReturnsAsync(clientes);

            var result = await _clienteService.GetAllClientes();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(c => c.Usuario == "usuarioTeste1"));
            Assert.IsTrue(result.Any(c => c.Usuario == "usuarioTeste2"));
        }

        [Test]
        public async Task GetClienteByUsuario_ShouldReturnCliente_WhenUsuarioExists_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(cliente.Usuario)).ReturnsAsync(cliente);

            var result = await _clienteService.GetClienteByUsuario(cliente.Usuario);

            Assert.AreEqual(cliente, result);
        }

        [Test]
        public async Task AddCliente_ShouldAddCliente_WhenUsuarioIsUnique_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(cliente.Usuario)).ReturnsAsync((Cliente)null);
            await _clienteService.AddCliente(cliente);
            _mockClienteRepository.Verify(repo => repo.AddCliente(cliente), Times.Once);
        }

        [Test]
        public async Task UpdateCliente_ShouldUpdateCliente_WhenUsuarioExists_TesteSucesso() 
        {
            var clienteExistente = new Cliente { Usuario = "usuarioTeste", Nome = "Nome original"};
            var clienteAtualizado = new Cliente { Usuario = "usuarioTeste", Nome = "Nome atualizado" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(clienteExistente.Usuario)).ReturnsAsync(clienteExistente);

            await _clienteService.UpdateCliente(clienteAtualizado);

            _mockClienteRepository.Verify(repo => repo.UpdateCliente(It.Is<Cliente>(
                c => c.Usuario == "usuarioTeste" &&
                     c.Nome == "Nome atualizado"
                     )), Times.Once);
        }

        [Test]
        public async Task DeleteCliente_ShouldDeleteCliente_WhenUsuarioExists_TesteSucesso()
        {
            var usuario = "usuarioTeste";
            _mockClienteRepository.Setup(repo => repo.DeleteCliente(usuario)).Returns(Task.CompletedTask);

            await _clienteService.DeleteCliente(usuario);

            _mockClienteRepository.Verify(repo => repo.DeleteCliente(usuario), Times.Once);
        }
        #endregion

        #region Testes de falha

        #endregion
    }
}