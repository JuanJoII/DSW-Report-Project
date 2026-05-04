using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using rp_back.DTOs;

namespace rp_back.Interfaces
{
    public interface IReporteService
    {
        Task<List<ReporteResumenDTO>> ObtenerReportesAsync(
            int? categoriaId = null,
            int? estadoId = null,
            DateTime? fechaDesde = null,
            string? municipio = null
        );
        Task<ReporteDetalleDTO?> ObtenerReportePorIdAsync(int id);
        Task<List<ReporteResumenDTO>> ObtenerReportesPorUsuarioAsync(Guid usuarioId);
        Task<ReporteDetalleDTO?> CrearReporteAsync(
            CrearReporteDTO crearReporteDto,
            Guid usuarioId
        );
    }
}
