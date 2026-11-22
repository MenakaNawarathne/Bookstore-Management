using System.ComponentModel.DataAnnotations;

namespace BookStoresBlazor.Data.Models;

public class Store
{
    [MaxLength(4)]
    public string StoreId { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string StoreName { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? StoreAddress { get; set; }

    [MaxLength(20)]
    public string? City { get; set; }

    [MaxLength(2)]
    public string? State { get; set; }

    [MaxLength(5)]
    public string? Zip { get; set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
