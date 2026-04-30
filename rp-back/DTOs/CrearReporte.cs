using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class CrearReporteDTO
    {
        public string Descripcion { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? DireccionTexto { get; set; }
    }
}