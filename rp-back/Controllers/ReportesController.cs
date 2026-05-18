using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReporteResumenDTO>>> ObtenerReportes([FromQuery] ReporteFiltrosDTO filtros)
        {
            var reportes = await _reporteService.ObtenerReportesAsync(filtros);
            return Ok(reportes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReporteDetalleDTO>> ObtenerReportePorId(int id)
        {
            var reporte = await _reporteService.ObtenerReportePorIdAsync(id);
            if (reporte == null)
            {
                return NotFound("Reporte no encontrado");
            }
            return Ok(reporte);
        }

        [HttpGet("usuario/mis-reportes")]
        public async Task<ActionResult<List<ReporteResumenDTO>>> ObtenerMisReportes()
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(usuarioIdString, out var usuarioId))
            {
                return Unauthorized("Usuario no autenticado");
            }

            var reportes = await _reporteService.ObtenerReportesPorUsuarioAsync(usuarioId);
            return Ok(reportes);
        }

        [HttpPost]
        public async Task<ActionResult<ReporteDetalleDTO>> CrearReporte(CrearReporteDTO crearReporteDto)
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(usuarioIdString, out var usuarioId))
            {
                return Unauthorized("Usuario no autenticado");
            }

            var reporte = await _reporteService.CrearReporteAsync(crearReporteDto, usuarioId);
            if (reporte == null)
            {
                return BadRequest("Error al crear el reporte");
            }

            return CreatedAtAction(nameof(ObtenerReportePorId), new { id = reporte.Id }, reporte);
        }

        [HttpPatch("{id}/estado/{estadoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CambiarEstado(int id, int estadoId, [FromBody] ComentarioRequest? request = null)
        {

            try
            {
                // Obtener comentario del body si existe
                string? comentario = request?.comentario;

                // DEBUG
                Console.WriteLine($"[DEBUG] CambiarEstado - id: {id}, estadoId: {estadoId}, comentario: '{comentario}'");
                Console.WriteLine($"[DEBUG] request es null: {request == null}, comentario es null: {comentario == null}");

                // ✅ VALIDACIONES DENTRO del try-catch
                var adminIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(adminIdString, out var adminId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        error = "Admin no autenticado",
                        reporte = (object?)null
                    });
                }

                var reporte = await _reporteService.CambiarEstadoAsync(id, estadoId, adminId, comentario);
                if (reporte == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        error = "Reporte o estado no encontrado",
                        reporte = (object?)null
                    });
                }

                // ✅ Retorno exitoso con estructura correcta
                return Ok(new
                {
                    success = true,
                    error = (string?)null,
                    reporte = new
                    {
                        id = reporte.Id,
                        estadoId = reporte.EstadoId,
                        nombreEstado = reporte.NombreEstado
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = "Error al cambiar estado: " + ex.Message,
                    reporte = (object?)null
                });
            }
        }
    }

    // DTO para recibir comentario en el body
    public class ComentarioRequest
    {
        public string? comentario { get; set; }
    }
}
