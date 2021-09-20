using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Acessment.Data;
using Biblioteca.Api.Acessment.Model;

namespace Biblioteca.Api.Acessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly BibliotecaApiAcessmentContext _context;

        public LivrosController(BibliotecaApiAcessmentContext context)
        {
            _context = context;
        }

        // GET: api/Livros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroResponse>>> GetLivro()
        {
            var livros = await _context.Livro.ToListAsync();

            // var livrosResponse = livros.Select(livro => new LivroResponse { Id = livro.Id, Titulo= livro.Titulo, Isbn=livro.Isbn, Ano=livro.Ano });
            var list = new List<LivroResponse>();
            foreach (var livro in livros)
            {
                var response = new LivroResponse();
                response.Id = livro.Id;
                response.Titulo = livro.Titulo;
                response.Isbn = livro.Isbn;
                response.Ano = livro.Ano;

                list.Add(response);
            }
           
            return list;
        }

        // GET: api/Livros/5
        [HttpGet("{id}/autores")]
        public async Task<ActionResult<List<AutorResponse>>> GetAutores(Guid id)
        {
            var livro = await _context.Livro.Include(x => x.Autores).FirstOrDefaultAsync(x => x.Id == id);

            if (livro == null)
            {
                return NotFound();
            }
            var idsDosAutores = livro.Autores.Select(x => x.AutoresId);
            var autores = _context.Autor.Where(x => idsDosAutores.Contains(x.Id));

            List<AutorResponse> list = new List<AutorResponse>();
            foreach(var autor in autores)
            {
                var resposta = new AutorResponse();
                resposta.Id = autor.Id;
                resposta.Nome = autor.Nome;
                list.Add(resposta);
            }
            return list;
        }
        public class AutorResponse
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivro(Guid id)
        {
            var livro = await _context.Livro.FindAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            return livro;
        }

        // PUT: api/Livros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(Guid id, Livro livro)
        {
            if (id != livro.Id)
            {
                return BadRequest();
            }

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Livros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(criarLivroRequest criarLivro)
        {
            var livro = new Livro();
            livro.Id = Guid.NewGuid();
            livro.Titulo = criarLivro.Titulo;
            livro.Isbn = criarLivro.Isbn;
            livro.Ano = criarLivro.Ano;

            var idsDosAutores = criarLivro.Autores;
            var autores = _context.Autor.Where(x => idsDosAutores.Contains(x.Id));
            
            foreach(var autor in autores)
            {
               livro.AssociarAutor(autor);
            }
           
            _context.Livro.Add(livro);
            await _context.SaveChangesAsync();

            var resposta = new LivroResponse();
            resposta.Id = livro.Id;
            resposta.Titulo = livro.Titulo;
            resposta.Isbn = livro.Isbn;
            resposta.Ano = livro.Ano;
            return CreatedAtAction("GetLivro", new { id = livro.Id }, resposta);
        }

        public class criarLivroRequest
        {
            //public Guid Id { get; set; }
            public string Titulo { get; set; }
            public string Isbn { get; set; }
            public string Ano { get; set; }
            public List<Guid> Autores { get; set; }
        }
        public class LivroResponse
        {
            public Guid Id { get; set; }
            public string Titulo { get; set; }
            public string Isbn { get; set; }
            public string Ano { get; set; }
            public List<AutorResponse> Autores { get; set; } = new List<AutorResponse>();

        }
        // DELETE: api/Livros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(Guid id)
        {
            var livro = await _context.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livro.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LivroExists(Guid id)
        {
            return _context.Livro.Any(e => e.Id == id);
        }
    }
}
