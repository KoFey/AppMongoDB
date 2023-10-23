namespace AppMongoDB.Models
{
    public class AuthorsFromCountryDTO
    {
        public string Author { get; set; }
        public List<Book> Books { get; set; }
    }
}
