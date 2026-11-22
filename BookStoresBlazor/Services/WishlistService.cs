using BookStoresBlazor.Data.Models;

namespace BookStoresBlazor.Services;

/// <summary>
/// Scoped Wishlist service providing Customer wishlist features without external dependencies.
/// </summary>
public class WishlistService
{
    private readonly List<Book> _items = new();

    public IReadOnlyList<Book> Items => _items.AsReadOnly();

    public int TotalCount => _items.Count;

    public event Action? OnWishlistChanged;

    public bool IsInWishlist(string bookId) => _items.Any(b => b.BookId == bookId);

    public void Add(Book book)
    {
        if (!_items.Any(b => b.BookId == book.BookId))
        {
            _items.Add(book);
            OnWishlistChanged?.Invoke();
        }
    }

    public void Remove(string bookId)
    {
        var item = _items.FirstOrDefault(b => b.BookId == bookId);
        if (item != null)
        {
            _items.Remove(item);
            OnWishlistChanged?.Invoke();
        }
    }

    public void Toggle(Book book)
    {
        if (IsInWishlist(book.BookId))
            Remove(book.BookId);
        else
            Add(book);
    }

    public void Clear()
    {
        _items.Clear();
        OnWishlistChanged?.Invoke();
    }
}
