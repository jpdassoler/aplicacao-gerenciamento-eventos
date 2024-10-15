using EventManagerBackend.Models;
using EventManagerBackend.Services;
using EventManagerBackend.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService) 
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes() 
        {
            var clientes = await _clienteService.GetAllClientes();
            return Ok(clientes);
        }

        [HttpGet("{usuario}")]
        public async Task<IActionResult> GetClienteByUsuario(string usuario) 
        {
            var cliente = await _clienteService.GetClienteByUsuario(usuario);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _clienteService.AddCliente(cliente);
                return CreatedAtAction(nameof(GetClienteByUsuario), new { usuario = cliente.Usuario }, cliente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }

        }

        [HttpPut("{usuario}")]
        public async Task<IActionResult> UpdateCliente(string usuario, [FromBody] UpdateClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _clienteService.UpdateCliente(usuario, clienteDTO);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e) 
            {
                return BadRequest(e.Message);
            }
            catch (Exception e) 
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
            
        }

        [HttpDelete("{usuario}")]
        public async Task<IActionResult> DeleteCliente(string usuario)
        {
            try
            {
                await _clienteService.DeleteCliente(usuario);
                return NoContent();
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var cliente = await _clienteService.GetClienteByUsuario(loginDTO.Usuario);

            if (cliente == null)
            {
                return NotFound("Usuário ou senha incorretos.");
            }

            if (cliente.Senha != loginDTO.Senha)
            {
                return BadRequest("Senha incorreta.");
            }

            var clienteRetorno = new Cliente
            {
                Usuario = cliente.Usuario,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Instagram = cliente.Instagram
            };

            return Ok(clienteRetorno);
        }
    }
}
