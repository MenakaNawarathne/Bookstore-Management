using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoresBlazor.Data.Models;

public class Book
{
    [MaxLength(6)]
    public string BookId { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(12)]
    public string? Type { get; set; }

    public int? PubId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Advance { get; set; }

    public int? Royalty { get; set; }

    public int? YtdSales { get; set; }

    public int Stock { get; set; } = 15;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Notes { get; set; }

    public DateTime? PublishDate { get; set; }

    public Publisher? Publisher { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
