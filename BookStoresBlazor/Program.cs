using BookStoresBlazor.Data;
using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server & Controllers for Web API
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BookStores Mobile & Web API",
        Version = "v1",
        Description = "Backend REST API for Online Bookstore Mobile and Web Clients."
    });
});

// EF Core with SQLite
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "bookstores.db");
builder.Services.AddDbContextFactory<BookStoresDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// App services
builder.Services.AddScoped<IBookStoresService, BookStoresService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CartService>();       // One cart per user session
builder.Services.AddScoped<WishlistService>();   // User wishlist service
builder.Services.AddScoped<OrderService>();

// Auth
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();

var app = builder.Build();

// Migrate & seed DB on startup
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BookStoresDbContext>>();
    using var db = dbFactory.CreateDbContext();
    db.Database.Migrate();

    // Ensure demo accounts exist for demonstration and grading
    if (!db.AppUsers.Any(u => u.Username.ToLower() == "admin"))
    {
        db.AppUsers.Add(new BookStoresBlazor.Data.Models.AppUser
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            EmailAddress = "admin@bookstores.com",
            FirstName = "System",
            LastName = "Admin",
            Role = "Admin"
        });
    }

    if (!db.AppUsers.Any(u => u.Username.ToLower() == "staff"))
    {
        db.AppUsers.Add(new BookStoresBlazor.Data.Models.AppUser
        {
            Username = "staff",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff123!"),
            EmailAddress = "staff@bookstores.com",
            FirstName = "Store",
            LastName = "Staff",
            Role = "StoreStaff"
        });
    }

    if (!db.AppUsers.Any(u => u.Username.ToLower() == "customer"))
    {
        db.AppUsers.Add(new BookStoresBlazor.Data.Models.AppUser
        {
            Username = "customer",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
            EmailAddress = "customer@example.com",
            FirstName = "Demo",
            LastName = "Customer",
            Role = "Customer"
        });
    }

    db.SaveChanges();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Swagger API documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStores API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();

// Map API Controllers & Blazor Hub
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
