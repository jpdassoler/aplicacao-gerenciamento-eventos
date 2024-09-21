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

        [TearDown]
        public void TearDown()
        {
            _context.Clientes.RemoveRange(_context.Clientes);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
