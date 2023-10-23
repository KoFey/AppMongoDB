using AppMongoDB.Models;
using AppMongoDB.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AppMongoDB.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService) =>
            _booksService = booksService;


        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            return Ok(id);
            var book = await _booksService.GetAsync();

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        public  IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Rq(string id)
        {
            switch (id)
            {
                case "1":
                    return Ok(_booksService.AveragePrice());
                case "2":
                    return Ok(_booksService.MostExpensiveBooks());
                case "3":
                    return Ok(_booksService.BookCountsByCategory());
                case "4":
                    return Ok(_booksService.BooksByAuthor());
                case "5":
                    return Ok(_booksService.BookCountsByAuthor());
                case "6":
                    return Ok(_booksService.ExpensiveBooksByAuthor());
                case "7":
                    return Ok(_booksService.MostPopularGenre());
                case "8":
                    return Ok(_booksService.AuthorsFromCountry("Россия"));
                default: return Ok("not found");
            }
        }


        public IActionResult CreateRandom()
        {
            CreateController createController = new CreateController("mongodb://root:example@mongodb:27017/", "BookStore","Books");
            createController.InsertRandomData(31);
            return Ok("ok");
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            //return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
            return Ok(_booksService.GetAsync(newBook.Id));
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
