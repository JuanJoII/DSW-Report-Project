using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class CrearReporteDTO
    {
        [Required, StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public int CategoriaId { get; set; }
        [Required, Range(-90, 90)]
        public decimal Latitud { get; set; }
        [Required, Range(-180, 180)]
        public decimal Longitud { get; set; }
        [Required, StringLength(255)]
        public string DireccionTexto { get; set; } = string.Empty;
    }
}