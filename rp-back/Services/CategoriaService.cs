using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ReportaSabanaDbContext _context;

        public CategoriaService(ReportaSabanaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearCategoriaAsync(CrearCategoria dTo)
        {
            var categoriaExistente = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre.ToLower() == dTo.Nombre.ToLower());
            if (categoriaExistente != null)
            {
                return false;
            }
            var categoria = new Categoria
            {
                Nombre = dTo.Nombre
            };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return categorias;
        }
    }
}