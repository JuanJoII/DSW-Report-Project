using System;
using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class ActualizarEstadoDTO
    {
        [Required]
        public int NuevoEstadoId { get; set; }

        [StringLength(1000)]
        public string Comentario { get; set; } = string.Empty;
    }
}
