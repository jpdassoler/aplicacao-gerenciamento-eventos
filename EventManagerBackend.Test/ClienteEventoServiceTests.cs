﻿
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
    public class ClienteEventoServiceTests
    {
        private Mock<IClienteEventoRepository> _mockClienteEventoRepository;
        private ClienteEventoService _clienteEventoService;

        [SetUp]
        public void Setup()
        {
            _mockClienteEventoRepository = new Mock<IClienteEventoRepository>();
            _clienteEventoService = new ClienteEventoService(_mockClienteEventoRepository.Object);
        }

        #region Testes de Sucesso
        [Test]
        public async Task GetAllClienteEventos_ShouldReturnAllClienteEventos_TesteSucesso()
        {
            var clienteEventos = new List<ClienteEvento>
            {
                new ClienteEvento { Usuario = "usuarioTeste1", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim },
                new ClienteEvento { Usuario = "usuarioTeste2", ID_Evento = 2, Ind_Comparecimento = EnumIndComparecimento.Organizador }
            };

            _mockClienteEventoRepository.Setup(repo => repo.GetAllClienteEventos()).ReturnsAsync(clienteEventos);

            var result = await _clienteEventoService.GetAllClienteEventos();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(ce => ce.Usuario == "usuarioTeste1" && ce.ID_Evento == 1));
            Assert.IsTrue(result.Any(ce => ce.Usuario == "usuarioTeste2" && ce.ID_Evento == 2));
        }

        [Test]
        public async Task GetClienteEventoById_ShouldReturnClienteEvento_WhenClienteEventoExists_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento)).ReturnsAsync(clienteEvento);

            var result = await _clienteEventoService.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento);

            Assert.IsNotNull(result);
            Assert.AreEqual(clienteEvento, result);
        }

        [Test]
        public async Task AddClienteEvento_ShouldAddClienteEvento_WhenClienteEventoIsValid_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            await _clienteEventoService.AddClienteEvento(clienteEvento);

            _mockClienteEventoRepository.Verify(repo => repo.AddClienteEvento(clienteEvento), Times.Once);
        }

        [Test]
        public async Task UpdateClienteEvento_ShouldUpdateClienteEvento_WhenClienteEventoExists_TesteSucesso()
        {
            var existingClienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            var dto = new UpdateClienteEventoDTO { Ind_Comparecimento = EnumIndComparecimento.Organizador };

            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById(existingClienteEvento.Usuario, existingClienteEvento.ID_Evento)).ReturnsAsync(existingClienteEvento);

            await _clienteEventoService.UpdateClienteEvento(existingClienteEvento.Usuario, existingClienteEvento.ID_Evento, dto);

            _mockClienteEventoRepository.Verify(repo => repo.UpdateClienteEvento(It.Is<ClienteEvento>(ce => ce.Ind_Comparecimento == EnumIndComparecimento.Organizador 
                                                && ce.Usuario == existingClienteEvento.Usuario && ce.ID_Evento == existingClienteEvento.ID_Evento)), Times.Once);
        }

        [Test]
        public async Task DeleteClienteEvento_ShouldDeleteClienteEvento_WhenClienteEventoExists_TesteSucesso()
        {
            var existingClienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById(existingClienteEvento.Usuario, existingClienteEvento.ID_Evento)).ReturnsAsync(existingClienteEvento);

            await _clienteEventoService.DeleteClienteEvento(existingClienteEvento.Usuario, existingClienteEvento.ID_Evento);

            _mockClienteEventoRepository.Verify(repo => repo.DeleteClienteEvento(existingClienteEvento.Usuario, existingClienteEvento.ID_Evento), Times.Once);
        }
        #endregion

        #region Testes de Falha
        [Test]
        public void AddClienteEvento_ShouldThrowArgumentException_WhenUsuarioNaoInformado()
        {
            var clienteEvento = new ClienteEvento { ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.AddClienteEvento(clienteEvento));
            Assert.AreEqual("Usuario é obrigatório.", ex.Message);
        }

        [Test]
        public void AddClienteEvento_ShouldThrowArgumentException_WhenIdEventoNaoInformado()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", Ind_Comparecimento = EnumIndComparecimento.Sim };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.AddClienteEvento(clienteEvento));
            Assert.AreEqual("ID do evento é obrigatório.", ex.Message);
        }

        [Test]
        public void AddClienteEvento_ShouldThrowArgumentException_WhenIndComparecimentoNaoInformado()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.AddClienteEvento(clienteEvento));
            Assert.AreEqual("O comparecimento deve ser Sim, Não, Talvez ou Organizador.", ex.Message);
        }

        [Test]
        public void UpdateClienteEvento_ShouldThrownArgumentException_WhenUsuarioNaoInformado()
        {
            var idEvento = 1;
            var dto = new UpdateClienteEventoDTO { Ind_Comparecimento = EnumIndComparecimento.Sim };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.UpdateClienteEvento(null, idEvento, dto));
            Assert.AreEqual("Usuario é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateClienteEvento_ShouldThrownArgumentException_WhenIdEventoNaoInformado()
        {
            var usuario = "usuarioTeste";
            var dto = new UpdateClienteEventoDTO { Ind_Comparecimento = EnumIndComparecimento.Sim };
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.UpdateClienteEvento(usuario, 0, dto));
            Assert.AreEqual("ID do evento é obrigatório.", ex.Message);
        }

        [Test]
        public void UpdateEndereco_ShouldThrownArgumentException_WhenEnderecoDoesNotExist()
        {
            var dto = new UpdateClienteEventoDTO { Ind_Comparecimento = EnumIndComparecimento.Sim };
            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById("teste",99)).ReturnsAsync((ClienteEvento)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.UpdateClienteEvento("teste",99, dto));
            Assert.AreEqual("Registro de Cliente_Evento não encontrado.", ex.Message);
        }

        [Test]
        public void DeleteClienteEvento_ShouldThrowArgumentException_WhenClienteEventoDosNotExist()
        {
            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById(null, 99)).ReturnsAsync((ClienteEvento)null);
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _clienteEventoService.DeleteClienteEvento(null, 99));
            Assert.AreEqual("Cliente_Evento não encontrado.", ex.Message);
        }

        [Test]
        public async Task GetAllClienteEventos_ShouldReturnNull_WhenClienteEventosDoesNotExist()
        {
            _mockClienteEventoRepository.Setup(repo => repo.GetAllClienteEventos()).ReturnsAsync(Enumerable.Empty<ClienteEvento>().AsQueryable());
            var result = await _clienteEventoService.GetAllClienteEventos();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetClienteEventoByID_ShouldReturnNull_WhenClienteEventoDoesNotExist()
        {
            _mockClienteEventoRepository.Setup(repo => repo.GetClienteEventoById(null, 99)).ReturnsAsync((ClienteEvento)null);

            var result = await _clienteEventoService.GetClienteEventoById(null, 99);

            Assert.IsNull(result);
        }
        #endregion
    }
}
