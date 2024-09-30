using DotNetEnv;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class ClienteEventoRepositoryTests
    {
        private AppDbContext _context;
        private ClienteEventoRepository _clienteEventoRepository;

        [SetUp]
        public void SetUp()
        {
            var projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            var envFilePath = Path.Combine(projectDirectory, "Environments", ".env");
            Env.Load(envFilePath);
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql($"Server={Env.GetString("MYSQL_DB_HOST")};" +
                       $"Database={Env.GetString("MYSQL_DB_NAME")};" +
                       $"User={Env.GetString("MYSQL_DB_USER")};" +
                       $"Password={Env.GetString("MYSQL_DB_PASSWORD")};",
                       new MySqlServerVersion(new Version(8, 0, 39)))
                       .Options;

            _context = new AppDbContext(options);
            _clienteEventoRepository = new ClienteEventoRepository(_context);

            //Limpar e preparar o banco de dados
            _context.Database.EnsureCreated();
        }

        [Test]
        public async Task AddClienteEvento_ShouldAddClienteEventoToDatabase_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };

            await _clienteEventoRepository.AddClienteEvento(clienteEvento);

            var result = await _context.ClienteEventos.FirstOrDefaultAsync(ce => ce.ID_Evento == clienteEvento.ID_Evento && ce.Usuario == clienteEvento.Usuario);
            Assert.IsNotNull(result);
            Assert.AreEqual(clienteEvento, result);
        }

        [Test]
        public async Task GetAllClienteEventos_ShouldReturnAllClienteEventos_TesteSucesso()
        {
            var clienteEvento1 = new ClienteEvento { Usuario = "usuarioTeste1", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            var clienteEvento2 = new ClienteEvento { Usuario = "usuarioTeste2", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Talvez };
            await _clienteEventoRepository.AddClienteEvento(clienteEvento1);
            await _clienteEventoRepository.AddClienteEvento(clienteEvento2);

            var result = await _clienteEventoRepository.GetAllClienteEventos();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(ce => ce.Usuario == clienteEvento1.Usuario && ce.ID_Evento == clienteEvento1.ID_Evento));
            Assert.IsTrue(result.Any(ce => ce.Usuario == clienteEvento2.Usuario && ce.ID_Evento == clienteEvento2.ID_Evento));
        }

        [Test]
        public async Task GetClienteEventoByID_ShouldReturnClienteEvento_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            await _clienteEventoRepository.AddClienteEvento(clienteEvento);

            var result = await _clienteEventoRepository.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento);

            Assert.IsNotNull(result);
            Assert.AreEqual(clienteEvento.Usuario, result.Usuario);
            Assert.AreEqual(clienteEvento.ID_Evento, result.ID_Evento);
            Assert.AreEqual(clienteEvento.Ind_Comparecimento, result.Ind_Comparecimento);
        }

        [Test]
        public async Task GetClienteEventoByID_ShouldReturnNull_WhenClienteEventoDoesNotExist_TesteFalha()
        {

            var result = await _clienteEventoRepository.GetClienteEventoById(null, 0);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateClienteEvento_ShouldUpdateClienteEvento_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            await _clienteEventoRepository.AddClienteEvento(clienteEvento);

            clienteEvento.Ind_Comparecimento = EnumIndComparecimento.Organizador;
            await _clienteEventoRepository.UpdateClienteEvento(clienteEvento);
            var result = await _clienteEventoRepository.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento);

            Assert.AreEqual(EnumIndComparecimento.Organizador, result.Ind_Comparecimento);
        }

        [Test]
        public async Task DeleteClienteEvento_ShouldRemoveClienteEvento_TesteSucesso()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            await _clienteEventoRepository.AddClienteEvento(clienteEvento);

            await _clienteEventoRepository.DeleteClienteEvento(clienteEvento.Usuario, clienteEvento.ID_Evento);

            var deletedClienteEvento = await _clienteEventoRepository.GetClienteEventoById(clienteEvento.Usuario, clienteEvento.ID_Evento);

            Assert.IsNull(deletedClienteEvento);
        }

        [Test]
        public async Task DeleteClienteEvento_ShouldDoNothing_WhenClienteEventoDoesNotExist_TesteFalha()
        {
            var clienteEvento = new ClienteEvento { Usuario = "usuarioTeste", ID_Evento = 1, Ind_Comparecimento = EnumIndComparecimento.Sim };
            await _clienteEventoRepository.AddClienteEvento(clienteEvento);

            await _clienteEventoRepository.DeleteClienteEvento(null, 0);

            var result = await _clienteEventoRepository.GetAllClienteEventos();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TearDown]
        public void TearDown()
        {
            _context.ClienteEventos.RemoveRange(_context.ClienteEventos);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
