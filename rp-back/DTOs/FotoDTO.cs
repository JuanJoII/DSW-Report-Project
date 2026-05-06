using System;
using System.ComponentModel.DataAnnotations;

namespace rp_back.DTOs
{
    public class FotoDTO
    {
        public int Id { get; set; }

        [Required, Url]
        public string Url { get; set; } = string.Empty;
    }
}
