using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Auth : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public Auth(IAuthService authService, ITokenService tokenService)
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
            return CreatedAtAction(nameof(Register), new { id = usuario.Id }, null);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaLoginDto>> Login(LoginDto loginDto)
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
    }
}