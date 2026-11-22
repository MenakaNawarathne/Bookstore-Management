namespace BookStoresBlazor.Data.Models;

/// <summary>
/// Transient cart item — held in CartService (scoped, in-memory).
/// Not persisted to the database.
/// </summary>
public class CartItem
{
    public string BookId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public int Stock { get; set; } = 99;
    public decimal LineTotal => UnitPrice * Quantity;
}
