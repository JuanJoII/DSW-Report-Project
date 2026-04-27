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