using BookStoresBlazor.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BookStoresBlazor.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private AppUser? _currentUser;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_currentUser is null)
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }

        var identity = BuildIdentity(_currentUser);
        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }

    public void Login(AppUser user)
    {
        _currentUser = user;
        var identity = BuildIdentity(user);
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(principal)));
    }

    public void Logout()
    {
        _currentUser = null;
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(anonymous)));
    }

    public AppUser? GetCurrentUser() => _currentUser;

    private static ClaimsIdentity BuildIdentity(AppUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FirstName", user.FirstName ?? string.Empty),
            new Claim("LastName", user.LastName ?? string.Empty),
        };
        return new ClaimsIdentity(claims, "BookStoresAuth");
    }
}
