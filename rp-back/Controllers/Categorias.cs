using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Categorias : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public Categorias(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("ListarTodas")]
        public async Task<ActionResult<IEnumerable<Categoria>>> ListarTodas()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<ActionResult> CrearCateoria([FromBody] CrearCategoria dTo)
        {
            if (string.IsNullOrEmpty(dTo.Nombre))
            {
                return BadRequest("El nombre de la categoría es obligatorio.");
            }

            var resultado = await _categoriaService.CrearCategoriaAsync(dTo);
            if (!resultado)
            {
                return Conflict("Ya existe una categoría con ese nombre.");
            }

            return Ok("Categoría creada exitosamente.");
        }
    }
}