using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/autores (controller = [Autores]Controller), desventaja: si se cambia el nombre del controlador, se cambia el nombre de la ruta -> los clientes tienen que actualizar la ruta que usan
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context) 
        {
            this.context = context;
        }

        [HttpGet] // api/autores
        [HttpGet("list")] // api/autores/list
        [HttpGet("/listado")] // list
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(autor=> autor.books).ToListAsync();
        }

        [HttpGet("first")]
        public async Task<ActionResult<Autor>> FirstAuthor()
        {
            return await context.Autores.Include(autor=> autor.books).FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param2=person}")] // si le pongo el = le pone el valor por defecto a param2, si le pongo ? por defecto es null
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
            //no buena práctica este if
            return await context.Autores.FirstOrDefaultAsync(x => x.Id == id) == null ? NotFound() : await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] string name)
        {
            return await context.Autores.FirstOrDefaultAsync(x => x.Name.Contains(name)) == null ? NotFound() : await context.Autores.FirstOrDefaultAsync(x => x.Name.Contains(name));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Autor author)
        {
            var existsAuthor = await context.Autores.AnyAsync(x => x.Name == author.Name);

            if (existsAuthor)
            {
                return BadRequest($"Author {author.Name} already exists.");
            }

            context.Add(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // api/autores/{autorid}
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            var existe = isAuthorExisting(id, this.context);

            if (existe)
            {
                //esta comprobación no tiene mucho sentido ya que se hace en isAuthorExisting
                if (id != autor.Id)
                {
                    return BadRequest("The author id is not the same");
                }
                
            }
            else
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")] // api/autores/{autorid}
        public async Task<ActionResult> Delete(int id)
        {
            var existe = isAuthorExisting(id, this.context);

            if (!existe) {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        private bool isAuthorExisting(int id, ApplicationDbContext context)
        {
            return context.Autores.Any(author=>author.Id == id);
        }
    }
}
