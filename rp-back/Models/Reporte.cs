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
        public virtual Usuario? Usuario { get; set; }


        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }


        [Required]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public EstadoReporte? Estado { get; set; }


        [Required, Column(TypeName = "text")]
        public string Descripcion { get; set; } = string.Empty;


        [StringLength(255)]
        public string? DireccionTexto { get; set; }


        [Column(TypeName = "decimal(9,6)")] 
        public decimal Latitud { get; set; }


        [Column(TypeName = "decimal(9,6)")]
        public decimal Longitud { get; set; }


        public DateTime FechaCreacion { get; set; } = DateTime.Now;


        public ICollection<Foto> Fotos { get; set; } = new List<Foto>();

    }
}