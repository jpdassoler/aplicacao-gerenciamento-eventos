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
        public async Task AddCliente_ShouldAddCliente_WhenUsuarioIsUnique_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(cliente.Usuario)).ReturnsAsync((Cliente)null);
            await _clienteService.AddCliente(cliente);
            _mockClienteRepository.Verify(repo => repo.AddCliente(cliente), Times.Once);
        }
        #endregion
    }
}