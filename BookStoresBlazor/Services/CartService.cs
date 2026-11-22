using BookStoresBlazor.Data.Models;

namespace BookStoresBlazor.Services;

/// <summary>
/// Scoped in-memory shopping cart service.
/// One instance per Blazor Server circuit (one per user session).
/// </summary>
public class CartService
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public int TotalCount => _items.Sum(i => i.Quantity);

    public decimal TotalAmount => _items.Sum(i => i.LineTotal);

    public event Action? OnCartChanged;

    public bool AddItem(string bookId, string title, decimal unitPrice, int quantity = 1, int availableStock = 99)
    {
        if (availableStock <= 0 || quantity <= 0) return false;

        var existing = _items.FirstOrDefault(i => i.BookId == bookId);
        if (existing is not null)
        {
            var targetQty = existing.Quantity + quantity;
            if (targetQty > availableStock)
            {
                existing.Quantity = availableStock;
            }
            else
            {
                existing.Quantity = targetQty;
            }
            existing.Stock = availableStock;
        }
        else
        {
            var initialQty = Math.Min(quantity, availableStock);
            _items.Add(new CartItem
            {
                BookId = bookId,
                Title = title,
                UnitPrice = unitPrice,
                Quantity = initialQty,
                Stock = availableStock
            });
        }

        OnCartChanged?.Invoke();
        return true;
    }

    public void RemoveItem(string bookId)
    {
        var item = _items.FirstOrDefault(i => i.BookId == bookId);
        if (item is not null)
        {
            _items.Remove(item);
            OnCartChanged?.Invoke();
        }
    }

    public bool UpdateQuantity(string bookId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.BookId == bookId);
        if (item is null) return false;

        if (quantity <= 0)
        {
            RemoveItem(bookId);
            return true;
        }

        if (quantity > item.Stock)
        {
            item.Quantity = item.Stock;
            OnCartChanged?.Invoke();
            return false;
        }

        item.Quantity = quantity;
        OnCartChanged?.Invoke();
        return true;
    }

    public void Clear()
    {
        _items.Clear();
        OnCartChanged?.Invoke();
    }
}
