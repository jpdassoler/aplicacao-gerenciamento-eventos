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
    public class EnderecoRepositoryTestes
    {
        private AppDbContext _context;
        private EnderecoRepository _enderecoRepository;

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
            _enderecoRepository = new EnderecoRepository(_context);

            //Limpar e preparar o banco de dados
            _context.Database.EnsureCreated();
        }

        [Test]
        public async Task AddEndereco_ShouldAddEnderecoToDataBase_TesteSucesso()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS", Numero = 123, Complemento = "apto" };
            await _enderecoRepository.AddEndereco(endereco);
            var result = await _context.Enderecos.FirstOrDefaultAsync(e => e.CEP == endereco.CEP);

            Assert.IsNotNull(result);
            Assert.AreEqual(endereco.Rua, result.Rua);
            Assert.AreEqual(endereco.Bairro, result.Bairro);
            Assert.AreEqual(endereco.Cidade, result.Cidade);
            Assert.AreEqual(endereco.UF, result.UF);
        }

        [Test]
        public async Task GetAllEnderecos_ShouldReturnAllEnderecos_TesteSucesso()
        {
            var endereco1 = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            var endereco2 = new Endereco { CEP = 87654321, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            await _enderecoRepository.AddEndereco(endereco1);
            await _enderecoRepository.AddEndereco(endereco2);

            var result = await _enderecoRepository.GetAllEnderecos();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(e => e.CEP == endereco1.CEP));
            Assert.IsTrue(result.Any(e => e.CEP == endereco2.CEP));
        }

        [Test]
        public async Task GetEnderecoByID_ShouldReturnEndereco_TesteSucesso()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            await _enderecoRepository.AddEndereco(endereco);

            var result = await _enderecoRepository.GetEnderecoById(endereco.ID_Endereco);

            Assert.IsNotNull(result);
            Assert.AreEqual(endereco.ID_Endereco, result.ID_Endereco);
            Assert.AreEqual(endereco.CEP, result.CEP);
            Assert.AreEqual(endereco.Rua, result.Rua);
        }

        [Test]
        public async Task GetEnderecoByID_ShouldReturnNull_WhenEnderecoDoesNotExist_TesteFalha()
        {

            var result = await _enderecoRepository.GetEnderecoById(0);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateEndereco_ShouldUpdateEndereco_TesteSucesso()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS"};
            await _enderecoRepository.AddEndereco(endereco);

            endereco.Rua = "Rua modificada";
            await _enderecoRepository.UpdateEndereco(endereco);
            var result = await _enderecoRepository.GetEnderecoById(endereco.ID_Endereco);

            Assert.AreEqual("Rua modificada", result.Rua);
        }

        [Test]
        public async Task DeleteEndereco_ShouldRemoveEndereco_TesteSucesso()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            await _enderecoRepository.AddEndereco(endereco);

            await _enderecoRepository.DeleteEndereco(endereco.ID_Endereco);

            var deletedEndereco = await _enderecoRepository.GetEnderecoById(endereco.ID_Endereco);

            Assert.IsNull(deletedEndereco);
        }

        [Test]
        public async Task DeleteEndereco_ShouldDoNothing_WhenEnderecoDoesNotExist_TesteFalha()
        {
            var endereco = new Endereco { CEP = 12345678, Rua = "Rua de Tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            await _enderecoRepository.AddEndereco(endereco);

            await _enderecoRepository.DeleteEndereco(0);

            var result = await _enderecoRepository.GetAllEnderecos();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Enderecos.RemoveRange(_context.Enderecos);
            _context.SaveChanges();
            _context.Dispose();
        }
    }
}
