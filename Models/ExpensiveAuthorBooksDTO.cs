namespace AppMongoDB.Models
{
    public class ExpensiveAuthorBooksDTO
    {
        public string Author { get; set; }
        public List<Book> Books { get; set; }
    }
}
