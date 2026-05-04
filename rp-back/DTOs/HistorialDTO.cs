using System;
using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class HistorialDTO
    {
        public int Id { get; set; }

        public int ReporteId { get; set; }

        public Guid AdminId { get; set; }

        [Required, StringLength(100)]
        public string NombreAdmin { get; set; } = string.Empty;

        public int EstadoNuevoId { get; set; }

        [Required, StringLength(50)]
        public string NombreEstadoNuevo { get; set; } = string.Empty;

        public string Comentario { get; set; } = string.Empty;

        public DateTime FechaCambio { get; set; }
    }
}