using BookStoresBlazor.Data.Models;

namespace BookStoresBlazor.Services;

public interface IUserService
{
    Task<AppUser?> LoginAsync(string username, string password);
    Task<AppUser?> RegisterAsync(string username, string password, string email, string? firstName = null, string? lastName = null);
    Task<AppUser?> GetUserAsync(int userId);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}
