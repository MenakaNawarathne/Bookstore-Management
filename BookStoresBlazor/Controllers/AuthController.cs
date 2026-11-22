using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresBlazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Username and password are required." });

        var user = await _userService.LoginAsync(request.Username, request.Password);
        if (user == null)
            return Unauthorized(new { message = "Invalid username or password." });

        return Ok(new
        {
            user.UserId,
            user.Username,
            user.EmailAddress,
            user.FirstName,
            user.LastName,
            user.Role
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { message = "Username, email, and password are required." });

        if (request.Password.Length < 8 || !request.Password.Any(char.IsLetter) || !request.Password.Any(char.IsDigit))
            return BadRequest(new { message = "Password must be at least 8 characters long and contain both letters and digits." });

        if (!System.Text.RegularExpressions.Regex.IsMatch(request.Username, "^[a-zA-Z0-9_.-]{3,50}$"))
            return BadRequest(new { message = "Username must be 3-50 characters and use only letters, numbers, dots, underscores or hyphens." });

        try
        {
            _ = new System.Net.Mail.MailAddress(request.Email.Trim());
        }
        catch
        {
            return BadRequest(new { message = "Please provide a valid email address." });
        }

        var user = await _userService.RegisterAsync(
            request.Username, request.Password, request.Email, request.FirstName, request.LastName);

        if (user == null)
            return Conflict(new { message = "An account with this username or email already exists." });

        return CreatedAtAction(nameof(Login), new
        {
            user.UserId,
            user.Username,
            user.EmailAddress,
            user.Role
        });
    }

    public record LoginDto(string Username, string Password);
    public record RegisterDto(string Username, string Email, string Password, string? FirstName = null, string? LastName = null);
}
