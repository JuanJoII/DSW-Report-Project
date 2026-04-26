using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rp_back.DTOs;
using rp_back.Models;

namespace rp_back.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Usuario usuario);
        Task<string> GenerarYGuardarRefreshToken(Usuario usuario);
        Task<RespuestaTokensDto?> RenovarTokensAsync(SolicitarRefreshTokenDto solicitarRefreshTokenDto);
    }
}