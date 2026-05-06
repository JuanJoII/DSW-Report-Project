using System;
using System.Threading.Tasks;
using rp_back.DTOs;

namespace rp_back.Interfaces
{
    public interface IAdminReporteService
    {
        Task<bool> ActualizarEstadoAsync(
            int reporteId,
            Guid adminId,
            ActualizarEstadoDTO actualizarEstadoDto
        );
        Task<EstadisticasDTO> ObtenerEstadisticasAsync();
    }
}
