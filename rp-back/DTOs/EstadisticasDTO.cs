using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class EstadisticasDTO
    {
        public int TotalReportes { get; set; }

        public List<ConteoCategoriasDTO> PorCategoria { get; set; } = new List<ConteoCategoriasDTO>();

        public List<ConteoEstadosDTO> PorEstado { get; set; } = new List<ConteoEstadosDTO>();
    }

    public class ConteoCategoriasDTO
    {
        public int CategoriaId { get; set; }

        [Required, StringLength(50)]
        public string NombreCategoria { get; set; } = string.Empty;

        public int Cantidad { get; set; }
    }

    public class ConteoEstadosDTO
    {
        public int EstadoId { get; set; }

        [Required, StringLength(50)]
        public string NombreEstado { get; set; } = string.Empty;

        public int Cantidad { get; set; }
    }
}
