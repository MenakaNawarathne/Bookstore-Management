using System.ComponentModel.DataAnnotations;

namespace BookStoresBlazor.Data.Models;

public class Author
{
    public int AuthorId { get; set; }

    [Required, MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(12)]
    public string? Phone { get; set; }

    [MaxLength(40)]
    public string? Address { get; set; }

    [MaxLength(20)]
    public string? City { get; set; }

    [MaxLength(2)]
    public string? State { get; set; }

    [MaxLength(5)]
    public string? Zip { get; set; }

    public bool Contract { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public string FullName => $"{FirstName} {LastName}";
}
