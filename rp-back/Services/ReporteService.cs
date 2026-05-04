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
    public class ReporteService : IReporteService
    {
        private readonly ReportaSabanaDbContext _context;

        public ReporteService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReporteResumenDTO>> ObtenerReportesAsync(int? categoriaId = null, int? estadoId = null, DateTime? fechaDesde = null, string? municipio = null)
        {
            var query = _context.Reportes.Include(r => r.Categoria).Include(r => r.Estado).Include(r => r.Fotos).AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(r => r.CategoriaId == categoriaId.Value);
            if (estadoId.HasValue)
                query = query.Where(r => r.EstadoId == estadoId.Value);
            if (fechaDesde.HasValue)
                query = query.Where(r => r.FechaCreacion >= fechaDesde.Value);
            if (!string.IsNullOrWhiteSpace(municipio))
                query = query.Where(r => r.DireccionTexto.Contains(municipio));

            var reportes = await query.ToListAsync();

            return reportes.Select(r => new ReporteResumenDTO
            {
                Id = r.Id,
                UsuarioId = r.UsuarioId,
                Descripcion = r.Descripcion,
                DireccionTexto = r.DireccionTexto,
                Latitud = r.Latitud,
                Longitud = r.Longitud,
                FechaCreacion = r.FechaCreacion,
                CategoriaId = r.CategoriaId,
                NombreCategoria = r.Categoria?.Nombre ?? "Sin categoría",
                EstadoId = r.EstadoId,
                NombreEstado = r.Estado?.Nombre ?? "Sin estado",
                TotalFotos = r.Fotos?.Count ?? 0
            }).ToList();
        }

        public async Task<ReporteDetalleDTO?> ObtenerReportePorIdAsync(int id)
        {
            var reporte = await _context.Reportes.Include(r => r.Categoria).Include(r => r.Estado).Include(r => r.Fotos).FirstOrDefaultAsync(r => r.Id == id);
            if (reporte == null)
                return null;

            var historial = await _context.Historiales.Include(h => h.Admin).Include(h => h.EstadoNuevo).Where(h => h.ReporteId == id).OrderByDescending(h => h.FechaCambio).ToListAsync();

            return new ReporteDetalleDTO
            {
                Id = reporte.Id,
                UsuarioId = reporte.UsuarioId,
                Descripcion = reporte.Descripcion,
                DireccionTexto = reporte.DireccionTexto,
                Latitud = reporte.Latitud,
                Longitud = reporte.Longitud,
                FechaCreacion = reporte.FechaCreacion,
                CategoriaId = reporte.CategoriaId,
                NombreCategoria = reporte.Categoria?.Nombre ?? "Sin categoría",
                EstadoId = reporte.EstadoId,
                NombreEstado = reporte.Estado?.Nombre ?? "Sin estado",
                Fotos = reporte.Fotos?.Select(f => new FotoDTO { Id = f.Id, Url = f.Url }).ToList() ?? new List<FotoDTO>(),
                Historial = historial.Select(h => new HistorialDTO
                {
                    Id = h.Id,
                    ReporteId = h.ReporteId,
                    AdminId = h.AdminId,
                    NombreAdmin = h.Admin?.NombreCompleto ?? "Admin desconocido",
                    EstadoNuevoId = h.EstadoNuevoId,
                    NombreEstadoNuevo = h.EstadoNuevo?.Nombre ?? "Estado desconocido",
                    Comentario = h.Comentario,
                    FechaCambio = h.FechaCambio
                }).ToList()
            };
        }

        public async Task<List<ReporteResumenDTO>> ObtenerReportesPorUsuarioAsync(Guid usuarioId)
        {
            var reportes = await _context.Reportes.Include(r => r.Categoria).Include(r => r.Estado).Include(r => r.Fotos).Where(r => r.UsuarioId == usuarioId).ToListAsync();

            return reportes.Select(r => new ReporteResumenDTO
            {
                Id = r.Id,
                UsuarioId = r.UsuarioId,
                Descripcion = r.Descripcion,
                DireccionTexto = r.DireccionTexto,
                Latitud = r.Latitud,
                Longitud = r.Longitud,
                FechaCreacion = r.FechaCreacion,
                CategoriaId = r.CategoriaId,
                NombreCategoria = r.Categoria?.Nombre ?? "Sin categoría",
                EstadoId = r.EstadoId,
                NombreEstado = r.Estado?.Nombre ?? "Sin estado",
                TotalFotos = r.Fotos?.Count ?? 0
            }).ToList();
        }

        public async Task<ReporteDetalleDTO?> CrearReporteAsync(CrearReporteDTO crearReporteDto, Guid usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return null;

            var categoria = await _context.Categorias.FindAsync(crearReporteDto.CategoriaId);
            if (categoria == null)
                return null;

            var estado = await _context.Estados.FirstOrDefaultAsync(e => e.Id == 1);
            if (estado == null)
                return null;

            var reporte = new Reporte
            {
                UsuarioId = usuarioId,
                CategoriaId = crearReporteDto.CategoriaId,
                EstadoId = estado.Id,
                Descripcion = crearReporteDto.Descripcion,
                DireccionTexto = crearReporteDto.DireccionTexto,
                Latitud = crearReporteDto.Latitud,
                Longitud = crearReporteDto.Longitud,
                FechaCreacion = DateTime.UtcNow,
                Usuario = usuario!,
                Categoria = categoria!,
                Estado = estado!
            };

            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();

            var reporteCreado = await _context.Reportes.Include(r => r.Categoria).Include(r => r.Estado).Include(r => r.Fotos).FirstOrDefaultAsync(r => r.Id == reporte.Id) ?? throw new InvalidOperationException("No se pudo crear el reporte");

            return new ReporteDetalleDTO
            {
                Id = reporteCreado.Id,
                UsuarioId = reporteCreado.UsuarioId,
                Descripcion = reporteCreado.Descripcion,
                DireccionTexto = reporteCreado.DireccionTexto,
                Latitud = reporteCreado.Latitud,
                Longitud = reporteCreado.Longitud,
                FechaCreacion = reporteCreado.FechaCreacion,
                CategoriaId = reporteCreado.CategoriaId,
                NombreCategoria = reporteCreado.Categoria?.Nombre ?? "Sin categoría",
                EstadoId = reporteCreado.EstadoId,
                NombreEstado = reporteCreado.Estado?.Nombre ?? "Sin estado",
                Fotos = reporteCreado.Fotos?.Select(f => new FotoDTO { Id = f.Id, Url = f.Url }).ToList() ?? new List<FotoDTO>(),
                Historial = new List<HistorialDTO>()
            };
        }
    }
}
