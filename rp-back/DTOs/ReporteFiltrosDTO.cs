using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class ReporteFiltrosDTO
    {
        public int? CategoriaId { get; set; }
        public int? EstadoId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public string? Municipio { get; set; }
    }
}