using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class RespuestaLoginDto
    {
        public required Guid Id { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}