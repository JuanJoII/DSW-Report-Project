using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Estados : ControllerBase
    {
        private readonly IEstadoService _estadoService;

        public Estados(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<EstadoReporte>>> ListarTodos()
        {
            var estados = await _estadoService.GetEstadosAsync();
            return Ok(estados);
        }

        [HttpPost]
        public async Task<ActionResult> CrearEstado([FromBody] CrearEstadoDTO dTo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _estadoService.CrearEstadoAsync(dTo);
            if (!resultado)
            {
                return Conflict("Ya existe un estado con ese nombre.");
            }

            return StatusCode(StatusCodes.Status201Created, new { message = "Estado creado exitosamente." });
        }
    }
}