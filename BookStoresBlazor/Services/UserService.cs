using BookStoresBlazor.Data;
using BookStoresBlazor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoresBlazor.Services;

public class UserService : IUserService
{
    private readonly IDbContextFactory<BookStoresDbContext> _contextFactory;

    public UserService(IDbContextFactory<BookStoresDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<AppUser?> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var identifier = username.Trim().ToLowerInvariant();
        var user = await ctx.AppUsers
            .FirstOrDefaultAsync(u => u.Username.ToLower() == identifier ||
                                      u.EmailAddress.ToLower() == identifier);

        if (user is null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return user;
    }

    public async Task<AppUser?> RegisterAsync(string username, string password, string email,
        string? firstName = null, string? lastName = null)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            return null;

        if (username.Trim().Length < 3 || username.Trim().Length > 50)
            return null;

        if (!System.Text.RegularExpressions.Regex.IsMatch(username.Trim(), "^[a-zA-Z0-9_.-]+$"))
            return null;

        if (password.Length < 8 || !password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            return null;

        try
        {
            var address = new System.Net.Mail.MailAddress(email.Trim());
            if (!string.Equals(address.Address, email.Trim(), StringComparison.OrdinalIgnoreCase))
                return null;
        }
        catch
        {
            return null;
        }

        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var normalizedUsername = username.Trim().ToLowerInvariant();
        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (await ctx.AppUsers.AnyAsync(u => u.Username.ToLower() == normalizedUsername ||
                                             u.EmailAddress.ToLower() == normalizedEmail))
        {
            return null;
        }

        var user = new AppUser
        {
            Username = username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            EmailAddress = email.Trim(),
            FirstName = firstName?.Trim(),
            LastName = lastName?.Trim(),
            // Public registration always creates a Customer account.
            // Administrator/Staff roles are assigned by the Administrator.
            Role = "Customer"
        };

        ctx.AppUsers.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task<AppUser?> GetUserAsync(int userId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.AppUsers.FindAsync(userId);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.AppUsers.AnyAsync(u => u.Username.ToLower() == username.Trim().ToLower());
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.AppUsers.AnyAsync(u => u.EmailAddress != null && u.EmailAddress.ToLower() == email.Trim().ToLower());
    }
}
