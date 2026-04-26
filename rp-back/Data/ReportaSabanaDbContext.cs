using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using rp_back.Models;

namespace rp_back.Data
{
    public class ReportaSabanaDbContext(DbContextOptions<ReportaSabanaDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
    }
}