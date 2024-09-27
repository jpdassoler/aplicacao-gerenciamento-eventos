using EventManagerBackend.DTOs;
using EventManagerBackend.Models;
using EventManagerBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventos()
        {
            var eventos = await _eventoService.GetAllEventos();
            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventoById(int id)
        {
            var evento = await _eventoService.GetEventoById(id);
            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvento([FromBody] Evento evento)
        {
            if (evento == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _eventoService.AddEvento(evento);
                return CreatedAtAction(nameof(GetEventoById), new { id = evento.ID_Evento }, evento);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] UpdateEventoDTO eventoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _eventoService.UpdateEvento(id, eventoDTO);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            try
            {
                await _eventoService.DeleteEvento(id);
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
