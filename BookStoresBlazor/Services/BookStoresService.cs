using BookStoresBlazor.Data;
using BookStoresBlazor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoresBlazor.Services;

public class BookStoresService : IBookStoresService
{
    private readonly IDbContextFactory<BookStoresDbContext> _contextFactory;

    public BookStoresService(IDbContextFactory<BookStoresDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // ── Authors ──────────────────────────────────────────────
    public async Task<List<Author>> GetAuthorsAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Authors.OrderBy(a => a.LastName).ToListAsync();
    }

    public async Task<Author?> GetAuthorAsync(int id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Authors
            .Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
            .FirstOrDefaultAsync(a => a.AuthorId == id);
    }

    public async Task<Author> CreateAuthorAsync(Author author)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Authors.Add(author);
        await ctx.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAuthorAsync(Author author)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Authors.Update(author);
        await ctx.SaveChangesAsync();
        return author;
    }

    public async Task DeleteAuthorAsync(int id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var author = await ctx.Authors.FindAsync(id);
        if (author is not null)
        {
            ctx.Authors.Remove(author);
            await ctx.SaveChangesAsync();
        }
    }

    // ── Publishers ────────────────────────────────────────────
    public async Task<List<Publisher>> GetPublishersAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Publishers.OrderBy(p => p.PublisherName).ToListAsync();
    }

    public async Task<Publisher?> GetPublisherAsync(int id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.PubId == id);
    }

    public async Task<Publisher> CreatePublisherAsync(Publisher publisher)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Publishers.Add(publisher);
        await ctx.SaveChangesAsync();
        return publisher;
    }

    public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Publishers.Update(publisher);
        await ctx.SaveChangesAsync();
        return publisher;
    }

    public async Task DeletePublisherAsync(int id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var pub = await ctx.Publishers.FindAsync(id);
        if (pub is not null)
        {
            ctx.Publishers.Remove(pub);
            await ctx.SaveChangesAsync();
        }
    }

    // ── Books ─────────────────────────────────────────────────
    public async Task<List<Book>> GetBooksAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Books
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetBookAsync(string id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Books
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.BookId == id);
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Books.Add(book);
        await ctx.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        ctx.Books.Update(book);
        await ctx.SaveChangesAsync();
        return book;
    }

    public async Task DeleteBookAsync(string id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var book = await ctx.Books.FindAsync(id);
        if (book is not null)
        {
            ctx.Books.Remove(book);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task UpdateBookStockAsync(string bookId, int newStock)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var book = await ctx.Books.FindAsync(bookId);
        if (book != null)
        {
            book.Stock = Math.Max(0, newStock);
            await ctx.SaveChangesAsync();
        }
    }

    // ── Sales ─────────────────────────────────────────────────
    public async Task<List<Sale>> GetSalesAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Sales
            .Include(s => s.Store)
            .Include(s => s.Book)
            .OrderByDescending(s => s.OrderDate)
            .ToListAsync();
    }

    // ── Counts & Low Stock ────────────────────────────────────
    public async Task<int> GetAuthorCountAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Authors.CountAsync();
    }

    public async Task<int> GetPublisherCountAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Publishers.CountAsync();
    }

    public async Task<int> GetBookCountAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Books.CountAsync();
    }

    public async Task<int> GetSaleCountAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Sales.CountAsync();
    }

    public async Task<int> GetLowStockCountAsync(int threshold = 5)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Books.CountAsync(b => b.Stock <= threshold);
    }

    public async Task<List<Book>> GetLowStockBooksAsync(int threshold = 5)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Books
            .Include(b => b.Publisher)
            .Where(b => b.Stock <= threshold)
            .OrderBy(b => b.Stock)
            .ToListAsync();
    }
}
