using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Api.Acessment.Model
{
    public class Autor
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public List<AutorLivro> Livros { get; set; }
    }

    public class AutorLivro
    {
        public Guid Id { get; set; }
        public Guid AutoresId { get; set; }
        public Guid LivrosId { get; set; }
    }
    public class Livro
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Isbn { get; set; }
        public string Ano { get; set; }
        public List<AutorLivro> Autores { get; set; } = new List<AutorLivro>();

        public void AssociarAutor(Autor autor)
        {
            AutorLivro autorLivro = new AutorLivro();
            autorLivro.Id = Guid.NewGuid();
            autorLivro.AutoresId = autor.Id;
            autorLivro.LivrosId = Id;
            Autores.Add(autorLivro);
        }
    }
}
