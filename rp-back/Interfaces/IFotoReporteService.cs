using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rp_back.DTOs;
using rp_back.Models;

namespace rp_back.Interfaces
{
    public interface IFotoReporteService
    {
        public Task<bool> FotoReporteAsync(CrearFotoReporteDTO dTo);
        public Task<IEnumerable<Foto>> ObtenerFotosPorReporteIdAsync(int reporteId);
    }
}