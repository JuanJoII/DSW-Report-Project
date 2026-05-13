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
        public async Task<ActionResult> CambiarEstado(int id, int estadoId)
        {
            var adminIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(adminIdString, out var adminId))
            {
                return Unauthorized("Admin no autenticado");
            }

            var reporte = await _reporteService.CambiarEstadoAsync(id, estadoId, adminId);
            if (reporte == null)
            {
                return NotFound("Reporte o estado no encontrado");
            }

            // Devolver respuesta simplificada para evitar problemas de serialización
            return Ok(new
            {
                id = reporte.Id,
                estadoId = reporte.EstadoId,
                nombreEstado = reporte.NombreEstado
            });
        }
    }
}
