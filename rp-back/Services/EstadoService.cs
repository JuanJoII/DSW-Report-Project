using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly ReportaSabanaDbContext _context;

        public EstadoService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearEstadoAsync(CrearEstadoDTO dTo)
        {
            var estadoExistente = await _context.Estados.FirstOrDefaultAsync(e => e.Nombre.ToLower() == dTo.Nombre.ToLower());
            if (estadoExistente != null)
            {
                return false;
            }
            var estado = new EstadoReporte
            {
                Nombre = dTo.Nombre
            };
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EstadoReporte>> GetEstadosAsync()
        {
            return await _context.Estados.ToListAsync();
        }
    }
}