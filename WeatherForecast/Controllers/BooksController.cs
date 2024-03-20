using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private ApplicationDbContext context;

        public BooksController(ApplicationDbContext context) 
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get (int id)
        {
            return await context.Books.Include(x => x.Autor).FirstOrDefaultAsync(book => book.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var existeAutor = await context.Autores.AnyAsync(autor => autor.Id == book.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"The author of this book does not exist. AuthorId: {book.AutorId}");
            }

            context.Add(book);
            context.SaveChanges();
            return Ok();
        }
    }
}
