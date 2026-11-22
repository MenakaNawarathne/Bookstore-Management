using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoresBlazor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 12, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    Contract = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PubId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublisherName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PubId);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    StoreName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    StoreAddress = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 12, nullable: true),
                    PubId = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Advance = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Royalty = table.Column<int>(type: "INTEGER", nullable: true),
                    YtdSales = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PubId",
                        column: x => x.PubId,
                        principalTable: "Publishers",
                        principalColumn: "PubId");
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    RoyaltyPercentage = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.AuthorId, x.BookId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreId = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    OrderNum = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PayTerms = table.Column<string>(type: "TEXT", maxLength: 12, nullable: true),
                    BookId = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Address", "City", "Contract", "FirstName", "LastName", "Phone", "State", "Zip" },
                values: new object[,]
                {
                    { 1, "10932 Bigge Rd.", "Menlo Park", true, "Johnson", "White", "408 496-7223", "CA", "94025" },
                    { 2, "309 63rd St.", "Oakland", true, "Marjorie", "Green", "415 986-7020", "CA", "94618" },
                    { 3, "589 Darwin Ln.", "Berkeley", true, "Cheryl", "Carson", "415 548-7723", "CA", "94705" },
                    { 4, "22 Cleveland Av.", "San Jose", true, "Michael", "O'Leary", "408 286-2428", "CA", "95128" },
                    { 5, "5420 College Av.", "Oakland", true, "Dean", "Straight", "415 834-2919", "CA", "94609" },
                    { 6, "10 Mississippi Dr.", "Lawrence", false, "Meander", "Smith", "913 843-0462", "KS", "66044" },
                    { 7, "6223 Bateman St.", "Berkeley", true, "Abraham", "Bennet", "415 658-9932", "CA", "94705" },
                    { 8, "3410 Blonde St.", "Palo Alto", true, "Ann", "Dull", "415 836-7128", "CA", "94301" },
                    { 9, "PO Box 792", "Covelo", true, "Burt", "Gringlesby", "707 938-6445", "CA", "95428" },
                    { 10, "18 Broadway Av.", "San Francisco", true, "Charlene", "Locksley", "415 585-4620", "CA", "94130" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PubId", "City", "Country", "PublisherName", "State" },
                values: new object[,]
                {
                    { 1, "Boston", "USA", "New Moon Books", "MA" },
                    { 2, "Washington", "USA", "Binnet & Hardley", "DC" },
                    { 3, "Berkeley", "USA", "Algodata Infosystems", "CA" },
                    { 4, "Chicago", "USA", "Five Lakes Publishing", "IL" },
                    { 5, "Dallas", "USA", "Ramona Publishers", "TX" }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "StoreId", "City", "State", "StoreAddress", "StoreName", "Zip" },
                values: new object[,]
                {
                    { "6380", "Seattle", "WA", "788 Catamaugus Av.", "Eric the Read Books", "98056" },
                    { "7066", "Tustin", "CA", "567 Pasadena Ave.", "Barnum's", "92789" },
                    { "7067", "Los Gatos", "CA", "577 First St.", "News & Brews", "96745" },
                    { "7131", "Remulade", "WA", "24-A Avogadro Way", "Doc-U-Mat: Quality Laundry and Books", "98014" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Advance", "Notes", "Price", "PubId", "PublishDate", "Royalty", "Title", "Type", "YtdSales" },
                values: new object[,]
                {
                    { "BU1032", 5000.00m, null, 19.99m, 2, new DateTime(1991, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "The Busy Executive's Database Guide", "business", 4095 },
                    { "BU1111", 5000.00m, null, 11.95m, 2, new DateTime(1991, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Cooking with Computers: Surreptitious Balance Sheets", "business", 3876 },
                    { "BU2075", 10125.00m, null, 2.99m, 3, new DateTime(1991, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, "You Can Combat Computer Stress!", "business", 18722 },
                    { "BU7832", 5000.00m, null, 19.99m, 2, new DateTime(1991, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Straight Talk About Computers", "business", 4095 },
                    { "MC2222", 0m, null, 19.99m, 2, new DateTime(1991, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Silicon Valley Gastronomic Treats", "mod_cook", 2032 },
                    { "MC3021", 15000.00m, null, 2.99m, 2, new DateTime(1991, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, "The Gourmet Microwave", "mod_cook", 22246 },
                    { "PC1035", 7000.00m, null, 22.95m, 3, new DateTime(1991, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, "But Is It User Friendly?", "popular_comp", 8780 },
                    { "PC8888", 8000.00m, null, 20.00m, 3, new DateTime(1994, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Secrets of Silicon Valley", "popular_comp", 4095 },
                    { "PS1372", 7000.00m, null, 21.59m, 4, new DateTime(1991, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Computer Phobic AND Non-Phobic Individuals: Behavior Variations", "psychology", 375 },
                    { "PS2091", 2275.00m, null, 10.95m, 4, new DateTime(1991, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Is Anger the Enemy?", "psychology", 2045 }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId", "AuthorOrder", "RoyaltyPercentage" },
                values: new object[,]
                {
                    { 1, "BU1032", 1, 60 },
                    { 2, "BU1032", 2, 40 },
                    { 3, "BU2075", 1, 100 },
                    { 4, "BU1111", 1, 60 },
                    { 5, "BU7832", 1, 100 },
                    { 6, "MC2222", 1, 100 },
                    { 7, "MC3021", 1, 75 },
                    { 8, "PC1035", 1, 100 },
                    { 9, "PC8888", 1, 50 },
                    { 10, "PS1372", 1, 75 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "SaleId", "BookId", "OrderDate", "OrderNum", "PayTerms", "Quantity", "StoreId" },
                values: new object[,]
                {
                    { 1, "BU1032", new DateTime(1994, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "6871", "Net 60", 5, "6380" },
                    { 2, "PS2091", new DateTime(1994, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "722a", "Net 60", 3, "6380" },
                    { 3, "PC8888", new DateTime(1993, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "A2976", "Net 30", 50, "7066" },
                    { 4, "PS2091", new DateTime(1994, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "QA7442.3", "ON invoice", 75, "7066" },
                    { 5, "PS2091", new DateTime(1991, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "D4482", "Net 60", 10, "7067" },
                    { 6, "PS2091", new DateTime(1994, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "N914008", "Net 30", 20, "7131" },
                    { 7, "PS2091", new DateTime(1993, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "P3087a", "Net 60", 20, "7131" },
                    { 8, "MC3021", new DateTime(1993, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "P3087a", "Net 60", 25, "7131" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_BookId",
                table: "BookAuthors",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PubId",
                table: "Books",
                column: "PubId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BookId",
                table: "Sales",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_StoreId",
                table: "Sales",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Publishers");
        }
    }
}
