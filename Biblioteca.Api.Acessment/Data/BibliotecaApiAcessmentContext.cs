using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Acessment.Model;

namespace Biblioteca.Api.Acessment.Data
{
    public class BibliotecaApiAcessmentContext : DbContext
    {
        public BibliotecaApiAcessmentContext (DbContextOptions<BibliotecaApiAcessmentContext> options)
            : base(options)
        {
        }

        public DbSet<Biblioteca.Api.Acessment.Model.Autor> Autor { get; set; }

        public DbSet<Biblioteca.Api.Acessment.Model.Livro> Livro { get; set; }
    }
}
