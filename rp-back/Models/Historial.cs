using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.Models
{
    [Table("Historial")]
    public class Historial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReporteId { get; set; }
        [ForeignKey("ReporteId")]
        public virtual Reporte? Reporte { get; set; }

        [Required]
        public Guid AdminId { get; set; } 
        
        [ForeignKey("AdminId")]
        public virtual Usuario? Admin { get; set; }

        [Required]
        public int EstadoNuevoId { get; set; }
        [ForeignKey("EstadoNuevoId")]
        public virtual EstadoReporte? EstadoNuevo { get; set; }

        [Column(TypeName = "text")]
        public string? Comentario { get; set; }

        public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
    }
}