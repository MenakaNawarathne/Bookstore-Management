using BookStoresBlazor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoresBlazor.Data;

public class BookStoresDbContext : DbContext
{
    public BookStoresDbContext(DbContextOptions<BookStoresDbContext> options)
        : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicit primary keys for non-convention names
        modelBuilder.Entity<Book>().HasKey(b => b.BookId);
        modelBuilder.Entity<Store>().HasKey(s => s.StoreId);

        // Order relationships
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(i => i.Book)
            .WithMany()
            .HasForeignKey(i => i.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // BookAuthor composite key
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.AuthorId, ba.BookId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        // Book -> Publisher
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PubId);

        // Sale -> Store
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Store)
            .WithMany(st => st.Sales)
            .HasForeignKey(s => s.StoreId);

        // Sale -> Book
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Book)
            .WithMany(b => b.Sales)
            .HasForeignKey(s => s.BookId);

        // Seed Publishers
        modelBuilder.Entity<Publisher>().HasData(
            new Publisher { PubId = 1, PublisherName = "New Moon Books", City = "Boston", State = "MA", Country = "USA" },
            new Publisher { PubId = 2, PublisherName = "Binnet & Hardley", City = "Washington", State = "DC", Country = "USA" },
            new Publisher { PubId = 3, PublisherName = "Algodata Infosystems", City = "Berkeley", State = "CA", Country = "USA" },
            new Publisher { PubId = 4, PublisherName = "Five Lakes Publishing", City = "Chicago", State = "IL", Country = "USA" },
            new Publisher { PubId = 5, PublisherName = "Ramona Publishers", City = "Dallas", State = "TX", Country = "USA" }
        );

        // Seed Authors
        modelBuilder.Entity<Author>().HasData(
            new Author { AuthorId = 1, FirstName = "Johnson", LastName = "White", Phone = "408 496-7223", Address = "10932 Bigge Rd.", City = "Menlo Park", State = "CA", Zip = "94025", Contract = true },
            new Author { AuthorId = 2, FirstName = "Marjorie", LastName = "Green", Phone = "415 986-7020", Address = "309 63rd St.", City = "Oakland", State = "CA", Zip = "94618", Contract = true },
            new Author { AuthorId = 3, FirstName = "Cheryl", LastName = "Carson", Phone = "415 548-7723", Address = "589 Darwin Ln.", City = "Berkeley", State = "CA", Zip = "94705", Contract = true },
            new Author { AuthorId = 4, FirstName = "Michael", LastName = "O'Leary", Phone = "408 286-2428", Address = "22 Cleveland Av.", City = "San Jose", State = "CA", Zip = "95128", Contract = true },
            new Author { AuthorId = 5, FirstName = "Dean", LastName = "Straight", Phone = "415 834-2919", Address = "5420 College Av.", City = "Oakland", State = "CA", Zip = "94609", Contract = true },
            new Author { AuthorId = 6, FirstName = "Meander", LastName = "Smith", Phone = "913 843-0462", Address = "10 Mississippi Dr.", City = "Lawrence", State = "KS", Zip = "66044", Contract = false },
            new Author { AuthorId = 7, FirstName = "Abraham", LastName = "Bennet", Phone = "415 658-9932", Address = "6223 Bateman St.", City = "Berkeley", State = "CA", Zip = "94705", Contract = true },
            new Author { AuthorId = 8, FirstName = "Ann", LastName = "Dull", Phone = "415 836-7128", Address = "3410 Blonde St.", City = "Palo Alto", State = "CA", Zip = "94301", Contract = true },
            new Author { AuthorId = 9, FirstName = "Burt", LastName = "Gringlesby", Phone = "707 938-6445", Address = "PO Box 792", City = "Covelo", State = "CA", Zip = "95428", Contract = true },
            new Author { AuthorId = 10, FirstName = "Charlene", LastName = "Locksley", Phone = "415 585-4620", Address = "18 Broadway Av.", City = "San Francisco", State = "CA", Zip = "94130", Contract = true }
        );

        // Seed Books
        modelBuilder.Entity<Book>().HasData(
            new Book { BookId = "BU1032", Title = "The Busy Executive's Database Guide", Type = "business", PubId = 2, Price = 19.99m, Advance = 5000.00m, Royalty = 10, YtdSales = 4095, Stock = 18, PublishDate = new DateTime(1991, 6, 12), Description = "An essential guide for executives navigating database management systems. Covers SQL fundamentals, data modelling, and decision-making strategies using data-driven insights." },
            new Book { BookId = "BU1111", Title = "Cooking with Computers: Surreptitious Balance Sheets", Type = "business", PubId = 2, Price = 11.95m, Advance = 5000.00m, Royalty = 10, YtdSales = 3876, Stock = 4, PublishDate = new DateTime(1991, 6, 9), Description = "A witty look at how technology intersects with business finance. Reveals creative accounting practices enabled by computing power." },
            new Book { BookId = "BU2075", Title = "You Can Combat Computer Stress!", Type = "business", PubId = 3, Price = 2.99m, Advance = 10125.00m, Royalty = 24, YtdSales = 18722, Stock = 25, PublishDate = new DateTime(1991, 6, 30), Description = "Practical strategies for reducing workplace stress caused by technology overload. A must-read for modern professionals overwhelmed by digital demands." },
            new Book { BookId = "BU7832", Title = "Straight Talk About Computers", Type = "business", PubId = 2, Price = 19.99m, Advance = 5000.00m, Royalty = 10, YtdSales = 4095, Stock = 3, PublishDate = new DateTime(1991, 6, 22), Description = "A no-nonsense introduction to computers for business users. Breaks down complex technical concepts into plain language for non-technical managers." },
            new Book { BookId = "MC2222", Title = "Silicon Valley Gastronomic Treats", Type = "mod_cook", PubId = 2, Price = 19.99m, Advance = 0m, Royalty = 12, YtdSales = 2032, Stock = 12, PublishDate = new DateTime(1991, 6, 9), Description = "A culinary journey through Silicon Valley's vibrant food scene. Features recipes and restaurant profiles inspired by tech culture." },
            new Book { BookId = "MC3021", Title = "The Gourmet Microwave", Type = "mod_cook", PubId = 2, Price = 2.99m, Advance = 15000.00m, Royalty = 24, YtdSales = 22246, Stock = 0, PublishDate = new DateTime(1991, 6, 18), Description = "Discover gourmet cooking techniques adapted for the microwave. Over 200 recipes that deliver restaurant-quality meals in minutes." },
            new Book { BookId = "PC1035", Title = "But Is It User Friendly?", Type = "popular_comp", PubId = 3, Price = 22.95m, Advance = 7000.00m, Royalty = 16, YtdSales = 8780, Stock = 15, PublishDate = new DateTime(1991, 6, 30), Description = "A critical evaluation of popular software and hardware from the user's perspective. Essential reading for UX designers and product managers." },
            new Book { BookId = "PC8888", Title = "Secrets of Silicon Valley", Type = "popular_comp", PubId = 3, Price = 20.00m, Advance = 8000.00m, Royalty = 10, YtdSales = 4095, Stock = 8, PublishDate = new DateTime(1994, 10, 5), Description = "An insider's account of Silicon Valley's rise to global dominance. Reveals the strategies, failures and breakthroughs behind the world's most innovative companies." },
            new Book { BookId = "PS1372", Title = "Computer Phobic AND Non-Phobic Individuals: Behavior Variations", Type = "psychology", PubId = 4, Price = 21.59m, Advance = 7000.00m, Royalty = 10, YtdSales = 375, Stock = 2, PublishDate = new DateTime(1991, 10, 21), Description = "A psychological study exploring why some individuals embrace computers while others fear them. Offers therapeutic frameworks for overcoming technology anxiety." },
            new Book { BookId = "PS2091", Title = "Is Anger the Enemy?", Type = "psychology", PubId = 4, Price = 10.95m, Advance = 2275.00m, Royalty = 12, YtdSales = 2045, Stock = 20, PublishDate = new DateTime(1991, 6, 15), Description = "An exploration of anger's role in modern life and how to channel it constructively. Combines clinical research with real-world case studies." }
        );

        // Seed Stores
        modelBuilder.Entity<Store>().HasData(
            new Store { StoreId = "6380", StoreName = "Eric the Read Books", StoreAddress = "788 Catamaugus Av.", City = "Seattle", State = "WA", Zip = "98056" },
            new Store { StoreId = "7066", StoreName = "Barnum's", StoreAddress = "567 Pasadena Ave.", City = "Tustin", State = "CA", Zip = "92789" },
            new Store { StoreId = "7067", StoreName = "News & Brews", StoreAddress = "577 First St.", City = "Los Gatos", State = "CA", Zip = "96745" },
            new Store { StoreId = "7131", StoreName = "Doc-U-Mat: Quality Laundry and Books", StoreAddress = "24-A Avogadro Way", City = "Remulade", State = "WA", Zip = "98014" }
        );

        // Seed Sales
        modelBuilder.Entity<Sale>().HasData(
            new Sale { SaleId = 1, StoreId = "6380", OrderNum = "6871", OrderDate = new DateTime(1994, 9, 14), Quantity = 5, PayTerms = "Net 60", BookId = "BU1032" },
            new Sale { SaleId = 2, StoreId = "6380", OrderNum = "722a", OrderDate = new DateTime(1994, 9, 13), Quantity = 3, PayTerms = "Net 60", BookId = "PS2091" },
            new Sale { SaleId = 3, StoreId = "7066", OrderNum = "A2976", OrderDate = new DateTime(1993, 5, 24), Quantity = 50, PayTerms = "Net 30", BookId = "PC8888" },
            new Sale { SaleId = 4, StoreId = "7066", OrderNum = "QA7442.3", OrderDate = new DateTime(1994, 9, 13), Quantity = 75, PayTerms = "ON invoice", BookId = "PS2091" },
            new Sale { SaleId = 5, StoreId = "7067", OrderNum = "D4482", OrderDate = new DateTime(1991, 9, 14), Quantity = 10, PayTerms = "Net 60", BookId = "PS2091" },
            new Sale { SaleId = 6, StoreId = "7131", OrderNum = "N914008", OrderDate = new DateTime(1994, 9, 14), Quantity = 20, PayTerms = "Net 30", BookId = "PS2091" },
            new Sale { SaleId = 7, StoreId = "7131", OrderNum = "P3087a", OrderDate = new DateTime(1993, 5, 29), Quantity = 20, PayTerms = "Net 60", BookId = "PS2091" },
            new Sale { SaleId = 8, StoreId = "7131", OrderNum = "P3087a", OrderDate = new DateTime(1993, 5, 29), Quantity = 25, PayTerms = "Net 60", BookId = "MC3021" }
        );

        // Seed BookAuthors
        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor { AuthorId = 1, BookId = "BU1032", AuthorOrder = 1, RoyaltyPercentage = 60 },
            new BookAuthor { AuthorId = 2, BookId = "BU1032", AuthorOrder = 2, RoyaltyPercentage = 40 },
            new BookAuthor { AuthorId = 3, BookId = "BU2075", AuthorOrder = 1, RoyaltyPercentage = 100 },
            new BookAuthor { AuthorId = 4, BookId = "BU1111", AuthorOrder = 1, RoyaltyPercentage = 60 },
            new BookAuthor { AuthorId = 5, BookId = "BU7832", AuthorOrder = 1, RoyaltyPercentage = 100 },
            new BookAuthor { AuthorId = 6, BookId = "MC2222", AuthorOrder = 1, RoyaltyPercentage = 100 },
            new BookAuthor { AuthorId = 7, BookId = "MC3021", AuthorOrder = 1, RoyaltyPercentage = 75 },
            new BookAuthor { AuthorId = 8, BookId = "PC1035", AuthorOrder = 1, RoyaltyPercentage = 100 },
            new BookAuthor { AuthorId = 9, BookId = "PC8888", AuthorOrder = 1, RoyaltyPercentage = 50 },
            new BookAuthor { AuthorId = 10, BookId = "PS1372", AuthorOrder = 1, RoyaltyPercentage = 75 }
        );
    }
}
