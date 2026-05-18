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
        private const string ESTADO_INICIAL = "Pendiente";

        public ReporteService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReporteResumenDTO>> ObtenerReportesAsync(ReporteFiltrosDTO filtros)
        {
            var query = _context.Reportes.Include(r => r.Categoria).Include(r => r.Estado).Include(r => r.Fotos).AsQueryable();

            if (filtros.CategoriaId.HasValue)
                query = query.Where(r => r.CategoriaId == filtros.CategoriaId.Value);
            if (filtros.EstadoId.HasValue)
                query = query.Where(r => r.EstadoId == filtros.EstadoId.Value);
            if (filtros.FechaDesde.HasValue)
                query = query.Where(r => r.FechaCreacion >= filtros.FechaDesde.Value);
            if (!string.IsNullOrWhiteSpace(filtros.Municipio))
                query = query.Where(r => r.DireccionTexto.Contains(filtros.Municipio));

            var reportes = await query.ToListAsync();

            return reportes.Select(MapToResumenDTO).ToList();
        }

        public async Task<ReporteDetalleDTO?> ObtenerReportePorIdAsync(int id)
        {
            var reporte = await _context.Reportes
                .Include(r => r.Categoria)
                .Include(r => r.Estado)
                .Include(r => r.Fotos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reporte == null)
                return null;

            var historial = await _context.Historiales
                .Include(h => h.Admin)
                .Include(h => h.EstadoNuevo)
                .Where(h => h.ReporteId == id)
                .OrderByDescending(h => h.FechaCambio)
                .ToListAsync();

            return MapToDetalleDTO(reporte, historial);
        }

        public async Task<List<ReporteResumenDTO>> ObtenerReportesPorUsuarioAsync(Guid usuarioId)
        {
            var reportes = await _context.Reportes
                .Include(r => r.Categoria)
                .Include(r => r.Estado)
                .Include(r => r.Fotos)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return reportes.Select(MapToResumenDTO).ToList();
        }

        public async Task<ReporteDetalleDTO?> CrearReporteAsync(CrearReporteDTO crearReporteDto, Guid usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return null;

            var categoria = await _context.Categorias.FindAsync(crearReporteDto.CategoriaId);
            if (categoria == null)
                return null;

            var estado = await _context.Estados.FirstOrDefaultAsync(e => e.Nombre == ESTADO_INICIAL);
            if (estado == null)
            {
                // Si no existe el estado "Pendiente", tomamos el primero como fallback o lanzamos error
                estado = await _context.Estados.FirstOrDefaultAsync();
                if (estado == null) return null;
            }

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
                Usuario = usuario,
                Categoria = categoria,
                Estado = estado,
                Fotos = new List<Foto>()
            };

            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();

            // Retornamos directamente usando el objeto en memoria, evitando el segundo round-trip a la DB
            return MapToDetalleDTO(reporte);
        }

        public async Task<ReporteDetalleDTO?> CambiarEstadoAsync(int id, int estadoId, Guid adminId, string? comentario = null)
        {
            try
            {
                var reporte = await _context.Reportes
                    .Include(r => r.Categoria)
                    .Include(r => r.Estado)
                    .Include(r => r.Fotos)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (reporte == null)
                    return null;

                var estado = await _context.Estados.FindAsync(estadoId);
                if (estado == null)
                    return null;

                reporte.EstadoId = estadoId;
                await _context.SaveChangesAsync();

                // Registrar el cambio en Historial con comentario (solo si tiene contenido)
                var historialEntry = new Historial
                {
                    ReporteId = id,
                    AdminId = adminId,
                    EstadoNuevoId = estadoId,
                    Comentario = string.IsNullOrWhiteSpace(comentario) ? null : comentario.Trim(), // ← Solo guardar si tiene contenido
                    FechaCambio = DateTime.UtcNow
                };
                _context.Historiales.Add(historialEntry);
                await _context.SaveChangesAsync();

                // CRÍTICO: Recargar el Estado para obtener el nombre correcto
                await _context.Entry(reporte).Reference(r => r.Estado).LoadAsync();

                // Obtener el historial completo actualizado para retornar
                var historialCompleto = await _context.Historiales
                    .Include(h => h.Admin)
                    .Include(h => h.EstadoNuevo)
                    .Where(h => h.ReporteId == id)
                    .OrderByDescending(h => h.FechaCambio)
                    .ToListAsync();

                return MapToDetalleDTO(reporte, historialCompleto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error en CambiarEstadoAsync: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private ReporteResumenDTO MapToResumenDTO(Reporte r)
        {
            return new ReporteResumenDTO
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
            };
        }

        private ReporteDetalleDTO MapToDetalleDTO(Reporte reporte, List<Historial>? historial = null)
        {
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
                Historial = historial?.Select(h => new HistorialDTO
                {
                    Id = h.Id,
                    ReporteId = h.ReporteId,
                    AdminId = h.AdminId,
                    NombreAdmin = h.Admin?.NombreCompleto ?? "Admin desconocido",
                    EstadoNuevoId = h.EstadoNuevoId,
                    NombreEstadoNuevo = h.EstadoNuevo?.Nombre ?? "Estado desconocido",
                    Comentario = h.Comentario,
                    FechaCambio = h.FechaCambio
                }).ToList() ?? new List<HistorialDTO>()
            };
        }
    }
}
