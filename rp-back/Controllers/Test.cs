using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Test : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public Test(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var usuario = await _authService.RegistrarUsuarioAsync(registerDto);
            if (usuario == null)
            {
                return BadRequest("Error al registrar el usuario");
            }
            return Ok(usuario);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaTokensDto>> Login(LoginDto loginDto)
        {
            var token = await _authService.LoginUsuarioAsync(loginDto);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [HttpPost("RefreshTokens")]
        public async Task<ActionResult<RespuestaTokensDto>> RefreshTokens(SolicitarRefreshTokenDto solicitarRefreshTokenDto)
        {
            var token = await _tokenService.RenovarTokensAsync(solicitarRefreshTokenDto);
            if (token == null || token.AccessToken == null || token.RefreshToken == null)
            {
                return Unauthorized("Refresh token inválido");
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet("Protected")]
        public IActionResult Protected()
        {
            return Ok("Acceso protegido concedido");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ProtectedAdmin")]
        public IActionResult ProtectedAdmin()
        {
            return Ok("Acceso protegido de administrador concedido");
        }

    }
}