using DotNetEnv;
using EventManagerBackend.Models;
using EventManagerBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class EventoRepositoryTests
    {
        private AppDbContext _context;
        private EventoRepository _eventoRepository;

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
            _eventoRepository = new EventoRepository(_context);

            //Limpar e preparar o banco de dados
            _context.Database.EnsureCreated();
        }

        [Test]
        public async Task GetAllEventos_ShouldReturnAllEventos_TesteSucesso()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };
           
            await _eventoRepository.AddEvento(evento);

            var result = await _eventoRepository.GetAllEventos();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(e => e.Nome == evento.Nome));
            Assert.IsTrue(result.Any(e => e.Descricao == evento.Descricao));
        }

        [Test]
        public async Task GetEventoById_ShouldReturnEvento_WhenEventoExists_TesteSucesso()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };

            await _eventoRepository.AddEvento(evento);

            var result = await _eventoRepository.GetEventoById(evento.ID_Evento);

            Assert.IsNotNull(result);
            Assert.AreEqual(evento.ID_Evento, result.ID_Evento);
            Assert.AreEqual(evento.Nome, result.Nome);
            Assert.AreEqual(evento.Descricao, result.Descricao);

        }

        [Test]
        public async Task GetEventoById_ShouldReturnNull_WhenEventoDoesNotExist_TesteFalha()
        {
            var result = await _eventoRepository.GetEventoById(0);

            Assert.IsNull(result);
        }

        [Test]
        public async Task AddEvento_ShouldAddEventoToDataBase_TesteSucesso()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };

            await _eventoRepository.AddEvento(evento);

            var result = await _context.Eventos.FirstOrDefaultAsync(e => e.ID_Evento == evento.ID_Evento);

            Assert.IsNotNull(result);
            Assert.AreEqual(evento.Nome, result.Nome);
            Assert.AreEqual(evento.Descricao, result.Descricao);
            Assert.AreEqual(evento.Data, result.Data);
            Assert.AreEqual(evento.Preco_Ingresso, result.Preco_Ingresso);
        }

        [Test]
        public async Task UpdateEvento_ShouldUpdateEvento_TesteSucesso()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };

            await _eventoRepository.AddEvento(evento);

            evento.Nome = "Nome modificado";
            await _eventoRepository.UpdateEvento(evento);
            var result = await _eventoRepository.GetEventoById(evento.ID_Evento);

            Assert.AreEqual("Nome modificado", result.Nome);
        }

        [Test]
        public async Task DeleteEvento_ShouldRemoveEvento_TesteSucesso()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };

            await _eventoRepository.AddEvento(evento);

            await _eventoRepository.DeleteEvento(evento.ID_Evento);

            var deletedEvento = await _eventoRepository.GetEventoById(evento.ID_Evento);

            Assert.IsNull(deletedEvento);
        }

        [Test]
        public async Task DeleteEvento_ShouldDoNothing_WhenEventoDoesNotExist_TesteFalha()
        {
            var endereco = new Endereco
            {
                CEP = 12345678,
                Rua = "Rua Teste",
                Bairro = "Bairro Teste",
                Cidade = "Cidade Teste",
                UF = "RS"
            };

            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var evento = new Evento
            {
                ID_Endereco = endereco.ID_Endereco,
                Nome = "Evento Teste",
                Data = DateTime.Now.AddDays(1),  // Evento no futuro
                Preco_Ingresso = 100.00m,
                Descricao = "Descrição do Evento Teste"
            };

            await _eventoRepository.AddEvento(evento);

            await _eventoRepository.DeleteEvento(0);

            var result = await _eventoRepository.GetAllEventos();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Enderecos.RemoveRange(_context.Enderecos);
            _context.Eventos.RemoveRange(_context.Eventos);
            _context.SaveChanges();
            _context.Dispose();
        }
    }            
}
