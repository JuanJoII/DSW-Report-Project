using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rp_back.DTOs;
using rp_back.Models;

namespace rp_back.Interfaces
{
    public interface ICategoriaService
    {
        public Task<IEnumerable<Categoria>> GetCategoriasAsync();
        public Task<bool> CrearCategoriaAsync(CrearCategoria dTo);
    }
}