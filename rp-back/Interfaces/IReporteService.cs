using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using rp_back.DTOs;

namespace rp_back.Interfaces
{
    public interface IReporteService
    {
        Task<List<ReporteResumenDTO>> ObtenerReportesAsync(ReporteFiltrosDTO filtros);
        Task<ReporteDetalleDTO?> ObtenerReportePorIdAsync(int id);
        Task<List<ReporteResumenDTO>> ObtenerReportesPorUsuarioAsync(Guid usuarioId);
        Task<ReporteDetalleDTO?> CrearReporteAsync(
            CrearReporteDTO crearReporteDto,
            Guid usuarioId
        );
        Task<ReporteDetalleDTO?> CambiarEstadoAsync(int id, int estadoId, Guid adminId);
    }
}
