using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/admin-reportes")]
    [Authorize(Roles = "Admin")]
    public class AdminReporteController : ControllerBase
    {
        private readonly IAdminReporteService _adminReporteService;

        public AdminReporteController(IAdminReporteService adminReporteService)
        {
            _adminReporteService = adminReporteService;
        }

        [HttpPut("actualizar-estado/{reporteId}")]
        public async Task<ActionResult<bool>> ActualizarEstado(int reporteId, ActualizarEstadoDTO actualizarEstadoDto)
        {
            var adminIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(adminIdString, out var adminId))
            {
                return Unauthorized("Admin no autenticado");
            }

            var resultado = await _adminReporteService.ActualizarEstadoAsync(reporteId, adminId, actualizarEstadoDto);
            if (!resultado)
            {
                return BadRequest("Error al actualizar el estado del reporte");
            }

            return Ok(true);
        }

        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasDTO>> ObtenerEstadisticas()
        {
            var estadisticas = await _adminReporteService.ObtenerEstadisticasAsync();
            return Ok(estadisticas);
        }
    }
}
