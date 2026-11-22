using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoresBlazor.Data.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    [MaxLength(6)]
    public string BookId { get; set; } = string.Empty;

    [MaxLength(80)]
    public string BookTitle { get; set; } = string.Empty;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal LineTotal => Quantity * UnitPrice;

    public Order? Order { get; set; }
    public Book? Book { get; set; }
}
