using BookStoresBlazor.Data.Models;
using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresBlazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorsController : ControllerBase
{
    private readonly IBookStoresService _bookService;

    public AuthorsController(IBookStoresService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _bookService.GetAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        var author = await _bookService.GetAuthorAsync(id);
        if (author == null)
            return NotFound(new { message = $"Author with ID {id} not found." });

        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] Author author)
    {
        if (string.IsNullOrWhiteSpace(author.FirstName) || string.IsNullOrWhiteSpace(author.LastName))
            return BadRequest(new { message = "First and last names are required." });

        var created = await _bookService.CreateAuthorAsync(author);
        return CreatedAtAction(nameof(GetAuthor), new { id = created.AuthorId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
    {
        if (id != author.AuthorId)
            return BadRequest(new { message = "Author ID mismatch." });

        var existing = await _bookService.GetAuthorAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Author with ID {id} not found." });

        await _bookService.UpdateAuthorAsync(author);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var existing = await _bookService.GetAuthorAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Author with ID {id} not found." });

        await _bookService.DeleteAuthorAsync(id);
        return NoContent();
    }
}
