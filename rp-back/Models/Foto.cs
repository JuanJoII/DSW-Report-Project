using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.Models
{
    [Table("Foto")]
    public class Foto
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int ReporteId { get; set; }
        [ForeignKey("ReporteId")]
        public Reporte? Reporte { get; set; }


        [Required, Url]
        public string Url { get; set; } = string.Empty;
    }

}