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

            return CreatedAtAction(nameof(ObtenerFotosPorReporteId), new { reporteId = dTo.ReporteId }, resultado);
        }

        [HttpGet("{reporteId}")]
        public async Task<IActionResult> ObtenerFotosPorReporteId(int reporteId)
        {
            var fotos = await _fotoReporteService.ObtenerFotosPorReporteIdAsync(reporteId);
            return Ok(fotos);
        }
    }
}