using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class FotoReporteService : IFotoReporteService
    {
        private readonly ReportaSabanaDbContext _context;

        public FotoReporteService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> FotoReporteAsync(CrearFotoReporteDTO dTo)
        {
            var reporteExiste = await _context.Reportes.FindAsync(dTo.ReporteId);

            if (reporteExiste == null)
            {
                return false;
            }

            var fotoReporte = new Foto
            {
                ReporteId = dTo.ReporteId,
                Url = dTo.Url
            };

            _context.Fotos.Add(fotoReporte);
            await _context.SaveChangesAsync();

            return true;
        }

        public Task<IEnumerable<Foto>> ObtenerFotosPorReporteIdAsync(int reporteId)
        {
            var fotos = _context.Fotos.Where(f => f.ReporteId == reporteId).AsEnumerable();
            return Task.FromResult(fotos);
        }
    }
}