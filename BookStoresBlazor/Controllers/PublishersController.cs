using BookStoresBlazor.Data.Models;
using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresBlazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PublishersController : ControllerBase
{
    private readonly IBookStoresService _bookService;

    public PublishersController(IBookStoresService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublishers()
    {
        var publishers = await _bookService.GetPublishersAsync();
        return Ok(publishers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPublisher(int id)
    {
        var publisher = await _bookService.GetPublisherAsync(id);
        if (publisher == null)
            return NotFound(new { message = $"Publisher with ID {id} not found." });

        return Ok(publisher);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] Publisher publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher.PublisherName))
            return BadRequest(new { message = "Publisher name is required." });

        var created = await _bookService.CreatePublisherAsync(publisher);
        return CreatedAtAction(nameof(GetPublisher), new { id = created.PubId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher publisher)
    {
        if (id != publisher.PubId)
            return BadRequest(new { message = "Publisher ID mismatch." });

        var existing = await _bookService.GetPublisherAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Publisher with ID {id} not found." });

        await _bookService.UpdatePublisherAsync(publisher);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePublisher(int id)
    {
        var existing = await _bookService.GetPublisherAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Publisher with ID {id} not found." });

        await _bookService.DeletePublisherAsync(id);
        return NoContent();
    }
}
