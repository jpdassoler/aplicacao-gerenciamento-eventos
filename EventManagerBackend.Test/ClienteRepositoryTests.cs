using DotNetEnv;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class ClienteRepositoryTests
    {
        private AppDbContext _context;
        private ClienteRepository _clienteRepository;

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
                       new MySqlServerVersion(new Version(8,0,39)))
                       .Options;

            _context = new AppDbContext(options);
            _clienteRepository = new ClienteRepository(_context);

            //Limpar e preparar o banco de dados
            _context.Database.EnsureCreated();            
        }

        [Test]
        public async Task AddCliente_ShouldAddClienteToDataBase_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto"};
            await _clienteRepository.AddCliente(cliente);
            var result = await _context.Clientes.FirstOrDefaultAsync(c => c.Usuario == cliente.Usuario);

            Assert.IsNotNull(result);
            Assert.AreEqual(cliente.Nome, result.Nome);
            Assert.AreEqual(cliente.Senha, result.Senha);
        }

        [Test]
        public async Task GetClienteByUsuario_ShouldReturnCliente_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            await _clienteRepository.AddCliente(cliente);

            var result = await _clienteRepository.GetClienteByUsuario(cliente.Usuario);

            Assert.IsNotNull(result);
            Assert.AreEqual(cliente.Usuario, result.Usuario);
            Assert.AreEqual(cliente.Nome, result.Nome);
        }

        [Test]
        public async Task GetAllClientes_ShouldReturnAllClientes_TesteSucesso()
        {
            var cliente1 = new Cliente { Usuario = "usuarioTeste1", Nome = "Fulano 1", Senha = "xpto" };
            var cliente2 = new Cliente { Usuario = "usuarioTeste2", Nome = "Fulano 2", Senha = "xpto" };
            await _clienteRepository.AddCliente(cliente1);
            await _clienteRepository.AddCliente(cliente2);

            var result = await _clienteRepository.GetAllClientes();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(c => c.Usuario == cliente1.Usuario));
            Assert.IsTrue(result.Any(c => c.Usuario == cliente2.Usuario));
            Assert.IsTrue(result.Any(c => c.Nome == cliente1.Nome));
            Assert.IsTrue(result.Any(c => c.Nome == cliente2.Nome));
        }

        [Test]
        public async Task UpdateCliente_ShouldUpdateCliente_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            await _clienteRepository.AddCliente(cliente);

            cliente.Nome = "Fulano atualizado";
            await _clienteRepository.UpdateCliente(cliente);

            var updatedCliente = await _clienteRepository.GetClienteByUsuario(cliente.Usuario);

            Assert.IsNotNull(updatedCliente);
            Assert.AreEqual("Fulano atualizado", updatedCliente.Nome);
        }

        [Test]
        public async Task DeleteCliente_ShouldRemoveCliente_TesteSucesso()
        {
            var cliente = new Cliente { Usuario = "usuarioTeste", Nome = "Fulano de Tal", Senha = "xpto" };
            await _clienteRepository.AddCliente(cliente);

            await _clienteRepository.DeleteCliente(cliente.Usuario);

            var deletedCliente = await _clienteRepository.GetClienteByUsuario(cliente.Usuario);

            Assert.IsNull(deletedCliente);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Clientes.RemoveRange(_context.Clientes);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
