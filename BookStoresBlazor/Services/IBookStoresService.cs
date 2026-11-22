using BookStoresBlazor.Data.Models;

namespace BookStoresBlazor.Services;

public interface IBookStoresService
{
    // Authors
    Task<List<Author>> GetAuthorsAsync();
    Task<Author?> GetAuthorAsync(int id);
    Task<Author> CreateAuthorAsync(Author author);
    Task<Author> UpdateAuthorAsync(Author author);
    Task DeleteAuthorAsync(int id);

    // Publishers
    Task<List<Publisher>> GetPublishersAsync();
    Task<Publisher?> GetPublisherAsync(int id);
    Task<Publisher> CreatePublisherAsync(Publisher publisher);
    Task<Publisher> UpdatePublisherAsync(Publisher publisher);
    Task DeletePublisherAsync(int id);

    // Books
    Task<List<Book>> GetBooksAsync();
    Task<Book?> GetBookAsync(string id);
    Task<Book> CreateBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task DeleteBookAsync(string id);
    Task UpdateBookStockAsync(string bookId, int newStock);

    // Sales
    Task<List<Sale>> GetSalesAsync();

    // Counts & Low Stock
    Task<int> GetAuthorCountAsync();
    Task<int> GetPublisherCountAsync();
    Task<int> GetBookCountAsync();
    Task<int> GetSaleCountAsync();
    Task<int> GetLowStockCountAsync(int threshold = 5);
    Task<List<Book>> GetLowStockBooksAsync(int threshold = 5);
}
