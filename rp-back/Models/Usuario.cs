using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.Models
{
    public enum RolUsuario
    {
        Ciudadano,
        Admin
    }

    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;


        [Required]
        public string PasswordHash { get; set; } = string.Empty;


        [Required]
        public RolUsuario Rol { get; set; } = RolUsuario.Ciudadano;


        [Required, StringLength(20)]
        public string Cedula { get; set; } = string.Empty;


        [Required, StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;


        [Phone, StringLength(15)]
        public string? Telefono { get; set; }


        [StringLength(255)]
        public string? DireccionResidencia { get; set; }

        public string? Refreshtoken { get; set; }

        public DateTime? RefreshtokenExpira { get; set; }


        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    }
}