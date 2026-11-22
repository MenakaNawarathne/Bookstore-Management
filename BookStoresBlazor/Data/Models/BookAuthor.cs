namespace BookStoresBlazor.Data.Models;

public class BookAuthor
{
    public int AuthorId { get; set; }
    public string BookId { get; set; } = string.Empty;
    public int AuthorOrder { get; set; }
    public int? RoyaltyPercentage { get; set; }

    public Author Author { get; set; } = null!;
    public Book Book { get; set; } = null!;
}
