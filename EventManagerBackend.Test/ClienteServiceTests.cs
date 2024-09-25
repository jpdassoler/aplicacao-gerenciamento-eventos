using EventManagerBackend.DTOs;
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
            var usuario = "usuarioTeste";
            var clienteAtualizado = new UpdateClienteDTO { Nome = "Nome atualizado" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(clienteExistente.Usuario)).ReturnsAsync(clienteExistente);

            await _clienteService.UpdateCliente(usuario, clienteAtualizado);

            _mockClienteRepository.Verify(repo => repo.UpdateCliente(It.Is<Cliente>(
                c => c.Usuario == "usuarioTeste" &&
                     c.Nome == "Nome atualizado"
                     )), Times.Once);
        }

        [Test]
        public async Task DeleteCliente_ShouldDeleteCliente_WhenUsuarioExists_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste" } ;

            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(cliente.Usuario)).ReturnsAsync(cliente);

            await _clienteService.DeleteCliente(cliente.Usuario);

            _mockClienteRepository.Verify(repo => repo.DeleteCliente(cliente.Usuario), Times.Once);
        }
        #endregion

        #region Testes de falha
        [Test]
        public void AddCliente_ShouldThrowArgumentException_WhenUsuarioNaoInformado()
        {
            var cliente = new Cliente { Nome = "Fulano de Tal", Senha = "xpto" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.AddCliente(cliente));
            Assert.AreEqual("Usuário é obrigatório.", ex.Message);
        }

        [Test]
        public void AddCliente_ShouldThrowArgumentException_WhenSenhaNaoInformada()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.AddCliente(cliente));
            Assert.AreEqual("Senha é obrigatória.", ex.Message);
        }

        [Test]
        public void AddCliente_ShouldThrowArgumentException_WhenNomeNaoInformado()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Senha = "xpto" };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.AddCliente(cliente));
            Assert.AreEqual("Nome é obrigatório.", ex.Message);
        }

        [Test]
        public void AddCliente_ShouldThrowArgumentException_WhenUsuarioAlreadyExists()
        {
            var clienteExistente = new Cliente { Usuario = "usuarioExistente", Nome = "Fulano de Tal", Senha = "xpto" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(clienteExistente.Usuario)).ReturnsAsync(clienteExistente);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.AddCliente(clienteExistente));
            Assert.AreEqual("Este nome de usuário já está em uso.", ex.Message);
        }

        [Test]
        public void UpdateCliente_ShouldThrowArgumentException_WhenUsuarioNaoInformado()
        {
            var cliente = new UpdateClienteDTO { Nome = "Nome alterado" };

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.UpdateCliente(null, cliente));
            Assert.AreEqual("Usuário é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateCliente_ShouldThrowArgumentException_WhenUsuarioDoesNotExists()
        {
            var usuario = "usuarioTeste";
            var cliente = new UpdateClienteDTO { Nome = "Nome alterado" };
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(usuario)).ReturnsAsync((Cliente)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.UpdateCliente(usuario, cliente));
            Assert.AreEqual("Cliente não encontrado.", ex.Message);
        }

        [Test]
        public void DeleteCliente_ShouldThrowArgumentException_WhenUsuarioDoesNotExist()
        {
            var usuario = "usuarioInexistente";
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(usuario)).ReturnsAsync((Cliente)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteService.DeleteCliente(usuario));
            Assert.AreEqual("Cliente não encontrado.", ex.Message);
        }


        [Test]
        public async Task GetAllClientes_ShouldReturnNull_WhenClienteDoesNotExist()
        {
            _mockClienteRepository.Setup(repo => repo.GetAllClientes()).ReturnsAsync(Enumerable.Empty<Cliente>().AsQueryable());
            var result = await _clienteService.GetAllClientes();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetClienteByUsuario_ShouldReturnNull_WhenUsuarioDoesNotExist()
        {
            var usuario = "usuarioInexistente";
            _mockClienteRepository.Setup(repo => repo.GetClienteByUsuario(usuario)).ReturnsAsync((Cliente)null);

            var result = await _clienteService.GetClienteByUsuario(usuario);

            Assert.IsNull(result);
        }
        #endregion
    }
}