using System;
using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class ReporteResumenDTO
    {
        public int Id { get; set; }

        public Guid UsuarioId { get; set; }

        [Required, StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string DireccionTexto { get; set; } = string.Empty;

        [Required, Range(-90, 90)]
        public decimal Latitud { get; set; }

        [Required, Range(-180, 180)]
        public decimal Longitud { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int CategoriaId { get; set; }

        [Required, StringLength(50)]
        public string NombreCategoria { get; set; } = string.Empty;

        public int EstadoId { get; set; }

        [Required, StringLength(50)]
        public string NombreEstado { get; set; } = string.Empty;

        public int TotalFotos { get; set; }
    }
}
