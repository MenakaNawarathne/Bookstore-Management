using BookStoresBlazor.Data.Models;
using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresBlazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookStoresService _bookService;

    public BooksController(IBookStoresService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] string? type)
    {
        var books = await _bookService.GetBooksAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            books = books.Where(b =>
                b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (b.Publisher?.PublisherName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.BookAuthors.Any(ba => ba.Author.FullName.Contains(search, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            books = books.Where(b => b.Type != null && b.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var result = books.Select(b => new
        {
            b.BookId,
            b.Title,
            b.Type,
            b.Price,
            b.Stock,
            b.Description,
            b.PublishDate,
            Publisher = b.Publisher?.PublisherName,
            Authors = b.BookAuthors.Select(ba => ba.Author.FullName).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        var b = await _bookService.GetBookAsync(id);
        if (b == null)
            return NotFound(new { message = $"Book with ID '{id}' was not found." });

        return Ok(new
        {
            b.BookId,
            b.Title,
            b.Type,
            b.Price,
            b.Stock,
            b.Description,
            b.PublishDate,
            b.Notes,
            Publisher = b.Publisher != null ? new { b.Publisher.PubId, b.Publisher.PublisherName, b.Publisher.City, b.Publisher.Country } : null,
            Authors = b.BookAuthors.Select(ba => new { ba.Author.AuthorId, ba.Author.FullName }).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        if (string.IsNullOrWhiteSpace(book.BookId) || string.IsNullOrWhiteSpace(book.Title))
            return BadRequest(new { message = "BookId and Title are required." });

        if (book.Price < 0)
            return BadRequest(new { message = "Price cannot be negative." });

        if (book.Stock < 0)
            return BadRequest(new { message = "Stock cannot be negative." });

        var created = await _bookService.CreateBookAsync(book);
        return CreatedAtAction(nameof(GetBook), new { id = created.BookId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(string id, [FromBody] Book book)
    {
        if (id != book.BookId)
            return BadRequest(new { message = "Book ID mismatch." });

        if (book.Price < 0 || book.Stock < 0)
            return BadRequest(new { message = "Price and Stock cannot be negative." });

        var existing = await _bookService.GetBookAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Book '{id}' does not exist." });

        await _bookService.UpdateBookAsync(book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var existing = await _bookService.GetBookAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Book '{id}' does not exist." });

        await _bookService.DeleteBookAsync(id);
        return NoContent();
    }
}
