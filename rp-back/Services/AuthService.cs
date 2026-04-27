using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class AuthService : IAuthService
    {
        private readonly ReportaSabanaDbContext _context;
        private readonly ITokenService _tokenService;
        public AuthService(ReportaSabanaDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<RespuestaLoginDto?> LoginUsuarioAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (usuario == null)
            {
                return null;
            }
            if (new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var response = new RespuestaLoginDto
            {
                Id = usuario.Id,
                AccessToken = _tokenService.GenerateAccessToken(usuario),
                RefreshToken = await _tokenService.GenerarYGuardarRefreshToken(usuario)
            };

            return response;
        }

        public async Task<Usuario?> RegistrarUsuarioAsync(RegisterDto registerDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email))
            {
                return null;
            }

            var nuevoUsuario = new Usuario();
            var PasswordHash = new PasswordHasher<Usuario>().HashPassword(nuevoUsuario, registerDto.Password);

            nuevoUsuario.Email = registerDto.Email;
            nuevoUsuario.PasswordHash = PasswordHash;
            nuevoUsuario.Cedula = registerDto.Cedula;
            nuevoUsuario.NombreCompleto = registerDto.NombreCompleto;
            nuevoUsuario.Telefono = registerDto.Telefono;
            nuevoUsuario.DireccionResidencia = registerDto.DireccionResidencia;

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return nuevoUsuario;
        }
    }
}