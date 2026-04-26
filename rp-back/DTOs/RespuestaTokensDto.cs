using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rp_back.DTOs
{
    public class RespuestaTokensDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}