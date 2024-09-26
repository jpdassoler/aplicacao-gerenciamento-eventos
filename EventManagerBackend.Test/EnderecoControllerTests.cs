using EventManagerBackend.Controllers;
using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerBackend.Test
{
    [TestFixture]
    public class EnderecoControllerTests
    {
        private Mock<IEnderecoService> _mockEnderecoService;
        private EnderecoController _enderecoController;

        [SetUp]
        public void Setup()
        {
            _mockEnderecoService = new Mock<IEnderecoService>();
            _enderecoController = new EnderecoController(_mockEnderecoService.Object);
        }

        #region Testes de sucesso
        [Test]
        public async Task GetAllEnderecos_ShouldReturnOkResult_WithEnderecoList_TesteSucesso()
        {
            var enderecos = new List<Endereco>
            {
                new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" },
                new Endereco { ID_Endereco = 2, CEP = 87654321, Rua = "Rua de tal 2", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" }
            };

            _mockEnderecoService.Setup(service => service.GetAllEnderecos()).ReturnsAsync(enderecos);

            var result = await _enderecoController.GetAllEnderecos();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(enderecos, okResult.Value);
        }

        [Test]
        public async Task GetEnderecoById_ShouldReturnOkResult_WithEndereco_WhenEnderecoExists_TesteSucesso()
        {
            var endereco = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            _mockEnderecoService.Setup(service => service.GetEnderecoById(endereco.ID_Endereco)).ReturnsAsync(endereco);

            var result = await _enderecoController.GetEnderecoById(endereco.ID_Endereco);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(endereco, okResult.Value);
        }

        [Test]
        public async Task AddEndereco_ShouldReturnCreatedAtActionResult_WhenEnderecoIsValid_TesteSucesso()
        {
            var endereco = new Endereco { ID_Endereco = 1, CEP = 12345678, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };

            _mockEnderecoService.Setup(service => service.AddEndereco(endereco)).Returns(Task.CompletedTask);

            var result = await _enderecoController.AddEndereco(endereco);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual(endereco, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateEndereco_ShouldReturnNoContent_WhenEnderecoIsUpdated_TesteSucesso()
        {
            var idEndereco = 1;
            var dto = new UpdateEnderecoDTO { Rua = "Rua Alterada", Cidade = "Cidade Alterada" };

            _mockEnderecoService.Setup(service => service.UpdateEndereco(idEndereco, dto)).Returns(Task.CompletedTask);

            var result = await _enderecoController.UpdateEndereco(idEndereco, dto);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [Test]
        public async Task DeleteEndereco_ShouldReturnNoContent_WhenEnderecoIsDeleted_TesteSucesso()
        {
            var idEndereco = 1;
            _mockEnderecoService.Setup(service => service.DeleteEndereco(idEndereco)).Returns(Task.CompletedTask);

            var result = await _enderecoController.DeleteEndereco(idEndereco);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
        #endregion

        #region Testes de falha
        [Test]
        public async Task GetEnderecoById_ShouldReturnNotFound_WhenEnderecoDoesNotExist_TesteFalha()
        {
            var idEndereco = 99;
            _mockEnderecoService.Setup(service => service.GetEnderecoById(idEndereco)).ReturnsAsync((Endereco)null);

            var result = await _enderecoController.GetEnderecoById(idEndereco);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task AddEndereco_ShouldReturnBadRequest_WhenEnderecoNotInformed_TesteFalha()
        {
            var result = await _enderecoController.AddEndereco(null);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task AddEndereco_ShouldReturnBadRequest_WhenCEPInvalido_TesteFalha()
        {
            var endereco = new Endereco { ID_Endereco = 1, CEP = 123456789, Rua = "Rua de tal", Bairro = "Bairro de tal", Cidade = "Porto Alegre", UF = "RS" };
            _enderecoController.ModelState.AddModelError("CEP", "O CEP deve ter 8 dígitos.");
            var result = await _enderecoController.AddEndereco(endereco);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateEndereco_ShouldReturnBadRequest_WhenCEPInvalido_TesteFalha()
        {
            _enderecoController.ModelState.AddModelError("CEP", "O CEP deve ter 8 dígitos.");
            var idEndereco = 1;
            var dto = new UpdateEnderecoDTO { CEP = 123456789 };

            var result = await _enderecoController.UpdateEndereco(idEndereco, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateEndereco_ShouldReturnNotFound_WhenEnderecoDoesNotExist_TesteFalha()
        {
            var idEndereco = 99;
            var dto = new UpdateEnderecoDTO { Rua = "Rua Alterada" };

            _mockEnderecoService.Setup(service => service.UpdateEndereco(idEndereco, dto)).ThrowsAsync(new ArgumentException("Endereço não encontrado."));

            var result = await _enderecoController.UpdateEndereco(idEndereco, dto);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Endereço não encontrado.", notFoundResult.Value);
        }

        [Test]
        public async Task DeleteEndereco_ShouldReturnNotFound_WhenEnderecoDoesNotExist_TesteFalha()
        {
            var idEndereco = 99;
            _mockEnderecoService.Setup(service => service.DeleteEndereco(idEndereco)).ThrowsAsync(new ArgumentException("Endereço não encontrado."));

            var result = await _enderecoController.DeleteEndereco(idEndereco);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Endereço não encontrado.", notFoundResult.Value);
        }
        #endregion
    }
}
