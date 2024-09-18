using EventManagerBackend.Models;
using EventManagerBackend.Services;
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
        public async Task<IActionResult> UpdateCliente(string usuario, [FromBody] Cliente cliente)
        {
            if (usuario != cliente.Usuario)
            {
                return BadRequest("O nome de usuário não pode ser alterado.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _clienteService.UpdateCliente(cliente);
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
            await _clienteService.DeleteCliente(usuario);
            return NoContent();
        }
    }
}
