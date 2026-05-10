using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class CrearEstadoDTO
    {
        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}