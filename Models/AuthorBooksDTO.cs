namespace AppMongoDB.Models
{
    public class AuthorBooksDTO
    {
        public string Author { get; set; }
        public List<Book> Books { get; set; }
    }
}
