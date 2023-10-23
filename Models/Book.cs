
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AppMongoDB.Models
{
    public enum Genre
    {
        Фантастика,
        Роман,
        Детектив,
        Приключения,
        Фэнтези,
        НаучнаяФантастика,
        Классика,
        Драма,
        Биография,
        История,
        Мистика,
        Поэзия,
        Философия,
        Комедия,
        Триллер,
        Ужасы,
        НаучноПопулярнаяЛитература,
        Психология,
        Саморазвитие,
        Политика,
        Юмор,
        ЛюбовныйРоман,
        Спорт,
        Кулинария,
        ИскусствоИДизайн,
        Путешествия,
        Религия,
        Фэншуй,
        Эзотерика,
        СовременнаяЛитература,
        БизнесИЭкономика,
        НаукаИОбразование
    }
    public class Book
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; } = null!;

        public int Price { get; set; }

        public string Category { get; set; } = null!;

        public List<string> genre { get; set; } = new List<string>();

        public Author Author { get; set; } = null!;
    }
}
