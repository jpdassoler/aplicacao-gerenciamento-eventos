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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(string id) 
        {
            var cliente = await _clienteService.GetClienteById(id);
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
            await _clienteService.AddCliente(cliente);
            return CreatedAtAction(nameof(GetClienteById), new { usuario = cliente.Usuario }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(string id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Usuario)
            {
                return BadRequest();
            }
            await _clienteService.UpdateCliente(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(string id)
        {
            await _clienteService.DeleteCliente(id);
            return NoContent();
        }
    }
}
