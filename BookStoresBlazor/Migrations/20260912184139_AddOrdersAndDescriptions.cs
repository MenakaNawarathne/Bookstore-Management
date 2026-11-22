using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoresBlazor.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersAndDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Books",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ShippingName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ShippingAddress = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ShippingCity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ShippingPostCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AppUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    BookTitle = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "BU1032",
                column: "Description",
                value: "An essential guide for executives navigating database management systems. Covers SQL fundamentals, data modelling, and decision-making strategies using data-driven insights.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "BU1111",
                column: "Description",
                value: "A witty look at how technology intersects with business finance. Reveals creative accounting practices enabled by computing power.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "BU2075",
                column: "Description",
                value: "Practical strategies for reducing workplace stress caused by technology overload. A must-read for modern professionals overwhelmed by digital demands.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "BU7832",
                column: "Description",
                value: "A no-nonsense introduction to computers for business users. Breaks down complex technical concepts into plain language for non-technical managers.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "MC2222",
                column: "Description",
                value: "A culinary journey through Silicon Valley's vibrant food scene. Features recipes and restaurant profiles inspired by tech culture.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "MC3021",
                column: "Description",
                value: "Discover gourmet cooking techniques adapted for the microwave. Over 200 recipes that deliver restaurant-quality meals in minutes.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "PC1035",
                column: "Description",
                value: "A critical evaluation of popular software and hardware from the user's perspective. Essential reading for UX designers and product managers.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "PC8888",
                column: "Description",
                value: "An insider's account of Silicon Valley's rise to global dominance. Reveals the strategies, failures and breakthroughs behind the world's most innovative companies.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "PS1372",
                column: "Description",
                value: "A psychological study exploring why some individuals embrace computers while others fear them. Offers therapeutic frameworks for overcoming technology anxiety.");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: "PS2091",
                column: "Description",
                value: "An exploration of anger's role in modern life and how to channel it constructively. Combines clinical research with real-world case studies.");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Books");
        }
    }
}
