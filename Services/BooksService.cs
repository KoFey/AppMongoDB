using AppMongoDB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppMongoDB.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
            _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
            
        }

        public async Task<List<Book>> GetAsync() => await (await _booksCollection.FindAsync( _ => true)).ToListAsync();

        public async Task<Book?> GetAsync(string id) => await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Book newBook) => await _booksCollection.InsertOneAsync(newBook);
        public async Task UpdateAsync(string id,Book updateBook) => await _booksCollection.ReplaceOneAsync(x=>x.Id==id,updateBook);
        public async Task RemoveAsync(string id) => await _booksCollection.DeleteOneAsync(x=>x.Id==id);

        // Средняя цена книг в каждой категории
        public List<CategoryAveragePriceDTO> AveragePrice()
        {
            // decimal don't use
            var result = _booksCollection.Aggregate()
                .Group(b => b.Category, g => new CategoryAveragePriceDTO()
                {
                    Category = g.Key,
                    AveragePrice = Math.Ceiling(g.Average(b => b.Price) * 100) / 100
                })
                .ToList();

            return result;
        }

        // Самая дорогая книга в каждой категорииA
        public List<MostExpensiveBookDTO> MostExpensiveBooks()
        {
            var result = _booksCollection.Aggregate()
                .SortByDescending(b => b.Price)
                .Group(b => b.Category, g => new MostExpensiveBookDTO() { Category = g.Key, MostExpensiveBook = g.First() })
                .ToList();

            return result;
        }

        // Количество книг в каждой категории
        public List<CategoryBookCountDTO> BookCountsByCategory()
        {
            var result = _booksCollection.Aggregate()
                .Group(b => b.Category, g => new CategoryBookCountDTO() { Category = g.Key, Count = g.Count() })
                .ToList();

            return result;
        }

        // Список книг каждого автора
        public List<AuthorBooksDTO> BooksByAuthor()
        {
            var result = _booksCollection.Aggregate()
                .Group(b => b.Author.fullName, g => new AuthorBooksDTO() { Author = g.Key, Books = g.ToList() })
                .ToList();

            return result;
        }

        // Количество книг каждого автора
        public List<AuthorBookCountDTO> BookCountsByAuthor()
        {
            var result = _booksCollection.Aggregate()
                .Group(b => b.Author.fullName, g => new AuthorBookCountDTO() { Author = g.Key, Count = g.Count() })
                .ToList();

            return result;
        }


        // Список авторов, у которых книги дороже $50
        public List<ExpensiveAuthorBooksDTO> ExpensiveBooksByAuthor()
        {
            var result = _booksCollection.Aggregate()
                .Match(b => b.Price > 50)
                .Group(b => b.Author.fullName, g => new ExpensiveAuthorBooksDTO() { Author = g.Key, Books = g.ToList() })
                .ToList();

            return result;
        }

        // Самый популярный жанр (наибольшее количество книг)
        public List<PopularGenreDTO> MostPopularGenre()
        {
            var result = _booksCollection.Aggregate()
                .Unwind(b => b.genre)
                .Group(new BsonDocument { { "_id", "$genre" }, { "count", new BsonDocument("$sum", 1) } })
                .Sort(new BsonDocument("count", -1))
                .Limit(1)
                .ToList()
                .Select(doc => new PopularGenreDTO { Genre = doc["_id"].AsString, Count = doc["count"].AsInt32 })
                .ToList();

            return result;
        }

        // Список авторов из определенной страны(например, США) :
        public List<AuthorsFromCountryDTO> AuthorsFromCountry(string country)
        {
            var result = _booksCollection.Aggregate()
                .Match(b => b.Author.country == country)
                .Group(b => b.Author.fullName, g => new AuthorsFromCountryDTO() { Author = g.Key, Books = g.ToList() })
                .ToList();

            return result;
        }

    }
}
