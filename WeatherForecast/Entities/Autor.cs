using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Autor
    {
        public int Id { get; set; }

        [FirstLetterUpperCase]
        public string Name { get; set; }
        public List<Book> books { get; set; }
    }
}
