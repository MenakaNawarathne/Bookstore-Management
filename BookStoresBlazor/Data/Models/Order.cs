using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoresBlazor.Data.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [MaxLength(30)]
    public string OrderNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    [MaxLength(100)]
    public string? ShippingName { get; set; }

    [MaxLength(100)]
    public string? CustomerEmail { get; set; }

    [MaxLength(25)]
    public string? CustomerPhone { get; set; }

    [MaxLength(200)]
    public string? ShippingAddress { get; set; }

    [MaxLength(50)]
    public string? ShippingCity { get; set; }

    [MaxLength(50)]
    public string? ShippingPostCode { get; set; }

    [MaxLength(30)]
    public string PaymentMethod { get; set; } = "Cash on Delivery";

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public AppUser? Customer { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
