using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        private readonly ReportaSabanaDbContext _context;

        public TokenService(IConfiguration configuration, ReportaSabanaDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GenerarYGuardarRefreshToken(Usuario usuario)
        {
            var refreshToken = GenerateRefreshToken();
            usuario.Refreshtoken = refreshToken;
            usuario.RefreshtokenExpira = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public string GenerateAccessToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<Usuario?> ValidarRefreshToken(Guid UserId, string refreshToken)
        {
            var usuario = await _context.Usuarios.FindAsync(UserId);
            if (usuario == null || usuario.Refreshtoken != refreshToken || usuario.RefreshtokenExpira <= DateTime.UtcNow)
            {
                return null;
            }
            return usuario;
        }

        public async Task<RespuestaTokensDto?> RenovarTokensAsync(SolicitarRefreshTokenDto solicitarRefreshTokenDto)
        {
            var usuario = await ValidarRefreshToken(solicitarRefreshTokenDto.UserId, solicitarRefreshTokenDto.RefreshToken);
            if (usuario == null)
            {
                return null;
            }

            var accessToken = GenerateAccessToken(usuario);
            var refreshToken = await GenerarYGuardarRefreshToken(usuario);

            return new RespuestaTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}