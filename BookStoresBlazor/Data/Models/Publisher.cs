using System.ComponentModel.DataAnnotations;

namespace BookStoresBlazor.Data.Models;

public class Publisher
{
    [Key]
    public int PubId { get; set; }

    [Required, MaxLength(40)]
    public string PublisherName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? City { get; set; }

    [MaxLength(2)]
    public string? State { get; set; }

    [MaxLength(30)]
    public string? Country { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
