using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.Models
{
    [Table("EstadoReporte")]
    public class EstadoReporte
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

}