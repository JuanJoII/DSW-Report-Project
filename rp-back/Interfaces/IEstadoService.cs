using System.Collections.Generic;
using System.Threading.Tasks;
using rp_back.DTOs;
using rp_back.Models;

namespace rp_back.Interfaces
{
    public interface IEstadoService
    {
        Task<IEnumerable<EstadoReporte>> GetEstadosAsync();
        Task<bool> CrearEstadoAsync(CrearEstadoDTO dTo);
    }
}