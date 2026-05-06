using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class CrearCategoria
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}