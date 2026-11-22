using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoresBlazor.Data.Models;

public class Sale
{
    [Key]
    public int SaleId { get; set; }

    [MaxLength(4)]
    public string StoreId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? OrderNum { get; set; }

    public DateTime OrderDate { get; set; }

    public int Quantity { get; set; }

    [MaxLength(12)]
    public string? PayTerms { get; set; }

    [MaxLength(6)]
    public string BookId { get; set; } = string.Empty;

    public Store? Store { get; set; }
    public Book? Book { get; set; }
}
