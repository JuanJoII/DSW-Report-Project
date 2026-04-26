using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class RegisterDto
    {
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Cedula { get; set; } = string.Empty;


        [Required, StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;


        [Phone, StringLength(15)]
        public string? Telefono { get; set; }


        [StringLength(255)]
        public string? DireccionResidencia { get; set; }
    }
}