using AppMongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AppMongoDB.Controllers
{
    public class CreateController : Controller
    {
        List<string> bookNames = new List<string>
        {
            "To Kill a Mockingbird",
            "1984",
            "Pride and Prejudice",
            "The Great Gatsby",
            "The Catcher in the Rye"
        };

        List<string> categories = new List<string>
        {
            "Fiction",
            "Science Fiction",
            "Classic",
            "Fiction",
            "Fiction"
        };

        List<string> genres = new List<string>
        {
            "Science Fiction",
            "Romance",
            "Mystery",
            "Adventure",
            "Fantasy",
            "Science Fiction",
            "Classic",
            "Drama",
            "Biography",
            "History",
            "Mystery",
            "Poetry",
            "Philosophy",
            "Comedy",
            "Thriller",
            "Horror",
            "Non-fiction",
            "Psychology",
            "Self-Help",
            "Politics",
            "Humor",
            "Literary Fiction",
            "Sports",
            "Cooking",
            "Art and Design",
            "Travel",
            "Religion",
            "Feng Shui",
            "Esoteric",
            "Contemporary Fiction",
            "Business and Economics",
            "Science and Education"
        };

        List<Author> authors = new List<Author>
        {
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Александр Пушкин", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Лев Толстой", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Федор Достоевский", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Иван Тургенев", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Антон Чехов", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Эрнест Хемингуэй", country = "США" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Джейн Остин", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Марк Твен", country = "США" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Чарльз Диккенс", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Джордж Оруэлл", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Уильям Шекспир", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Дж.Р.Р. Толкин", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Джордж Мартин", country = "США" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Артур Конан Дойль", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Агата Кристи", country = "Великобритания" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Габриэль Гарсия Маркес", country = "Колумбия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Тони Моррисон", country = "США" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Харпер Ли", country = "США" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Айн Рэнд", country = "Россия" },
            new Author { Id = ObjectId.GenerateNewId().ToString(), fullName = "Джон Стейнбек", country = "США" }
        };

        public string fullName { get; set; }
        
        public string country { get; set; }


        private IMongoDatabase _database;
        private IMongoCollection<BsonDocument> _collection;

        public CreateController(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<BsonDocument>(collectionName);
        }

        public void InsertRandomData(int numberOfDocuments)
        {
            var random = new Random();
            var documents = new BsonDocument();
            HashSet<string> uniqueGenres = new HashSet<string>();
            for (int i = 0; i < numberOfDocuments; i++)
            {
                var Genres = new List<string>();
                
                var book = new Book
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    BookName = bookNames[random.Next(bookNames.Count)],
                    Price = random.Next(1, 100),
                    Category = categories[random.Next(categories.Count)],
                    genre = new List<string>(),
                    Author = authors[random.Next(authors.Count)]
                };

                while (book.genre.Count < random.Next(1,genres.Count))
                {
                    int randomIndex = random.Next(genres.Count);
                    string randomGenre = genres[randomIndex];

                    if (uniqueGenres.Add(randomGenre))
                    {
                        book.genre.Add(randomGenre);
                    }
                }

                // Очистка списка уникальных жанров для следующей книги
                uniqueGenres.Clear();

                _collection.InsertOne(book.ToBsonDocument());
            }
            
        }
    }
}
