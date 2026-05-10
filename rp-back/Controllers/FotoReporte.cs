using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FotoReporte : ControllerBase
    {
        private readonly IFotoReporteService _fotoReporteService;

        public FotoReporte(IFotoReporteService fotoReporteService)
        {
            _fotoReporteService = fotoReporteService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearFotoReporte([FromBody] CrearFotoReporteDTO dTo)
        {
            var resultado = await _fotoReporteService.FotoReporteAsync(dTo);

            if (!resultado)
            {
                return NotFound("Reporte no encontrado para asociar la foto.");
            }

            return StatusCode(StatusCodes.Status201Created, new { message = "Foto de reporte creada exitosamente." });
        }

        [HttpGet("{reporteId}")]
        public async Task<IActionResult> ObtenerFotosPorReporteId(int reporteId)
        {
            var fotos = await _fotoReporteService.ObtenerFotosPorReporteIdAsync(reporteId);
            return Ok(fotos);
        }

        [HttpGet("presignedUrl")]
        public async Task<IActionResult> ObtenerUrlFirmada([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("El nombre del archivo es requerido.");
            }

            try
            {
                var (uploadUrl, publicUrl) = await _fotoReporteService.GenerarUrlFirmadaAsync(fileName);
                return Ok(new { uploadUrl, publicUrl });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}