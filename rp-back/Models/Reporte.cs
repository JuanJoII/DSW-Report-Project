using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.Models
{
    [Table("Reporte")]
    public class Reporte
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public Guid UsuarioId { get; set; } 

        [ForeignKey("UsuarioId")] 
        public virtual required Usuario Usuario { get; set; }

        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual required Categoria Categoria { get; set; }

        [Required]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public virtual required EstadoReporte Estado { get; set; }

        [Required, Column(TypeName = "text")]
        public required string Descripcion { get; set; } 

        [Required, StringLength(255)]
        public required string DireccionTexto { get; set; } 

        [Required, Column(TypeName = "decimal(9,6)")]
        public decimal Latitud { get; set; }

        [Required, Column(TypeName = "decimal(9,6)")]
        public decimal Longitud { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<Foto> Fotos { get; set; } = new List<Foto>();

    }
}