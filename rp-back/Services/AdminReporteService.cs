using System;
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
    public class AdminReporteService : IAdminReporteService
    {
        private readonly ReportaSabanaDbContext _context;

        public AdminReporteService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ActualizarEstadoAsync(int reporteId, Guid adminId, ActualizarEstadoDTO actualizarEstadoDto)
        {
            var reporte = await _context.Reportes.FindAsync(reporteId);
            if (reporte == null)
                return false;

            var admin = await _context.Usuarios.FindAsync(adminId);
            if (admin == null || admin.Rol != RolUsuario.Admin)
                return false;

            var estado = await _context.Estados.FindAsync(actualizarEstadoDto.NuevoEstadoId);
            if (estado == null)
                return false;

            reporte.EstadoId = actualizarEstadoDto.NuevoEstadoId;
            _context.Reportes.Update(reporte);

            var historial = new Historial
            {
                ReporteId = reporteId,
                AdminId = adminId,
                EstadoNuevoId = actualizarEstadoDto.NuevoEstadoId,
                Comentario = actualizarEstadoDto.Comentario,
                FechaCambio = DateTime.UtcNow,
                Admin = admin!
            };

            _context.Historiales.Add(historial);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EstadisticasDTO> ObtenerEstadisticasAsync()
        {
            var total = await _context.Reportes.CountAsync();

            var reportesConCategoria = await _context.Reportes.Include(r => r.Categoria).ToListAsync();
            var porCategoria = reportesConCategoria.GroupBy(r => r.CategoriaId).Select(g => new ConteoCategoriasDTO
            {
                CategoriaId = g.Key,
                NombreCategoria = g.First().Categoria?.Nombre ?? "Sin categoría",
                Cantidad = g.Count()
            }).ToList();

            var reportesConEstado = await _context.Reportes.Include(r => r.Estado).ToListAsync();
            var porEstado = reportesConEstado.GroupBy(r => r.EstadoId).Select(g => new ConteoEstadosDTO
            {
                EstadoId = g.Key,
                NombreEstado = g.First().Estado?.Nombre ?? "Sin estado",
                Cantidad = g.Count()
            }).ToList();

            return new EstadisticasDTO
            {
                TotalReportes = total,
                PorCategoria = porCategoria,
                PorEstado = porEstado
            };
        }
    }
}
