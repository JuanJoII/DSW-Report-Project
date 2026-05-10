using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class CrearFotoReporteDTO
    {
        [Required]
        public int ReporteId { get; set; }

        [Required, Url]
        public required string Url { get; set; }
    }
}