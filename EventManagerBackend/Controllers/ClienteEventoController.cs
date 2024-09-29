using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClienteEventoController : ControllerBase
    {
        private readonly IClienteEventoService _clienteEventoService;
        public ClienteEventoController(IClienteEventoService clienteEventoService)
        {
            _clienteEventoService = clienteEventoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClienteEventos()
        {
            var clienteEventos = await _clienteEventoService.GetAllClienteEventos();
            return Ok(clienteEventos);
        }

        [HttpGet("{usuario}/{idEvento}")]
        public async Task<IActionResult> GetClienteEventoById(string usuario, int idEvento)
        {
            var clienteEvento = await _clienteEventoService.GetClienteEventoById(usuario, idEvento);
            if (clienteEvento == null)
            {
                return NotFound();
            }
            return Ok(clienteEvento);
        }

        [HttpPost]
        public async Task<IActionResult> AddClienteEvento([FromBody] ClienteEvento clienteEvento)
        {
            if (clienteEvento == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _clienteEventoService.AddClienteEvento(clienteEvento);
                return CreatedAtAction(nameof(GetClienteEventoById), new { usuario = clienteEvento.Usuario, idEvento = clienteEvento.ID_Evento}, clienteEvento);
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

        [HttpPut("{usuario}/{idEvento}")]
        public async Task<IActionResult> UpdateClienteEvento(string usuario, int idEvento, [FromBody] UpdateClienteEventoDTO clienteEvento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _clienteEventoService.UpdateClienteEvento(usuario, idEvento, clienteEvento);
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

        [HttpDelete("{usuario}/{idEvento}")]
        public async Task<IActionResult> DeleteClienteEvento(string usuario, int idEvento)
        {
            try
            {
                await _clienteEventoService.DeleteClienteEvento(usuario, idEvento);
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
    }
}
