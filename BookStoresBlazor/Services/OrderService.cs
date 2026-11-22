using BookStoresBlazor.Data;
using BookStoresBlazor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoresBlazor.Services;

public class OrderService
{
    private readonly IDbContextFactory<BookStoresDbContext> _contextFactory;

    public static readonly string[] ValidStatuses =
    { "Pending", "Confirmed", "Packed", "Shipped", "Delivered", "Cancelled" };

    private static readonly IReadOnlyDictionary<string, string[]> AllowedTransitions =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["Pending"] = new[] { "Confirmed", "Cancelled" },
            ["Confirmed"] = new[] { "Packed", "Cancelled" },
            ["Packed"] = new[] { "Shipped", "Cancelled" },
            ["Shipped"] = new[] { "Delivered", "Cancelled" },
            ["Delivered"] = Array.Empty<string>(),
            ["Cancelled"] = Array.Empty<string>()
        };

    public OrderService(IDbContextFactory<BookStoresDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Order> PlaceOrderAsync(
        int customerId, string shippingName, string shippingAddress, string shippingCity,
        string shippingPostCode, string customerEmail, string customerPhone,
        string paymentMethod, IEnumerable<CartItem> cartItems)
    {
        var itemsList = cartItems?.ToList() ?? new List<CartItem>();
        if (itemsList.Count == 0)
            throw new InvalidOperationException("Cannot place an order with an empty cart.");

        if (itemsList.Any(i => string.IsNullOrWhiteSpace(i.BookId) || i.Quantity <= 0))
            throw new InvalidOperationException("Each order item must have a valid book and quantity.");

        if (string.IsNullOrWhiteSpace(shippingName) || string.IsNullOrWhiteSpace(shippingAddress) ||
            string.IsNullOrWhiteSpace(shippingCity) || string.IsNullOrWhiteSpace(shippingPostCode))
            throw new InvalidOperationException("Complete shipping details are required.");

        var allowedPaymentMethods = new[] { "Cash on Delivery", "Card Payment (Demo)" };
        if (!allowedPaymentMethods.Contains(paymentMethod, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("Please select a valid payment method.");

        await using var ctx = await _contextFactory.CreateDbContextAsync();
        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var customer = await ctx.AppUsers.FirstOrDefaultAsync(u => u.UserId == customerId);
        if (customer is null)
            throw new InvalidOperationException("The customer account could not be found.");

        if (!string.Equals(customer.Role, "Customer", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Only customer accounts can place orders.");

        var orderItems = new List<OrderItem>();

        foreach (var ci in itemsList)
        {
            var book = await ctx.Books.FirstOrDefaultAsync(b => b.BookId == ci.BookId);
            if (book is null)
                throw new InvalidOperationException($"Book '{ci.BookId}' no longer exists.");

            if (book.Stock < ci.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{book.Title}'. Available: {book.Stock}, Requested: {ci.Quantity}.");

            var price = book.Price ?? 0m;
            if (price < 0)
                throw new InvalidOperationException($"Book '{book.Title}' has an invalid price.");

            book.Stock -= ci.Quantity;
            book.YtdSales = (book.YtdSales ?? 0) + ci.Quantity;

            orderItems.Add(new OrderItem
            {
                BookId = book.BookId,
                BookTitle = book.Title,
                Quantity = ci.Quantity,
                UnitPrice = price
            });
        }

        var order = new Order
        {
            CustomerId = customerId,
            OrderNumber = $"BK-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid():N}"[..24].ToUpperInvariant(),
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            ShippingName = shippingName.Trim(),
            ShippingAddress = shippingAddress.Trim(),
            ShippingCity = shippingCity.Trim(),
            ShippingPostCode = shippingPostCode.Trim(),
            CustomerEmail = customerEmail.Trim(),
            CustomerPhone = customerPhone.Trim(),
            PaymentMethod = paymentMethod,
            TotalAmount = orderItems.Sum(i => i.UnitPrice * i.Quantity),
            Items = orderItems
        };

        ctx.Orders.Add(order);
        await ctx.SaveChangesAsync();
        await transaction.CommitAsync();
        return order;
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<(bool Success, string Message)> UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        var requested = newStatus?.Trim() ?? string.Empty;
        var canonicalStatus = ValidStatuses.FirstOrDefault(s =>
            string.Equals(s, requested, StringComparison.OrdinalIgnoreCase));

        if (canonicalStatus is null)
            return (false, $"Invalid status: '{newStatus}'.");

        await using var ctx = await _contextFactory.CreateDbContextAsync();
        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var order = await ctx.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order is null) return (false, "Order not found.");

        if (string.Equals(order.Status, canonicalStatus, StringComparison.OrdinalIgnoreCase))
            return (true, $"Order #{orderId} is already {canonicalStatus}.");

        if (!AllowedTransitions.TryGetValue(order.Status, out var allowed) ||
            !allowed.Contains(canonicalStatus, StringComparer.OrdinalIgnoreCase))
        {
            return (false, $"Order cannot move from {order.Status} to {canonicalStatus}.");
        }

        if (canonicalStatus == "Cancelled")
        {
            foreach (var item in order.Items)
            {
                var book = await ctx.Books.FindAsync(item.BookId);
                if (book != null) book.Stock += item.Quantity;
            }
        }

        order.Status = canonicalStatus;
        await ctx.SaveChangesAsync();
        await transaction.CommitAsync();
        return (true, $"Order #{orderId} status changed to {canonicalStatus}.");
    }

    public async Task<(int TotalOrders, int PendingOrders, int ProcessingOrders, int ShippedOrders, int DeliveredOrders, decimal TotalSales)> GetOrderStatsAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var orders = await ctx.Orders.ToListAsync();

        var totalOrders = orders.Count;
        var pending = orders.Count(o => o.Status == "Pending");
        var processing = orders.Count(o => o.Status == "Confirmed" || o.Status == "Packed");
        var shipped = orders.Count(o => o.Status == "Shipped");
        var delivered = orders.Count(o => o.Status == "Delivered");
        var totalSales = orders.Where(o => o.Status != "Cancelled").Sum(o => o.TotalAmount);

        return (totalOrders, pending, processing, shipped, delivered, totalSales);
    }

    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.AppUsers.OrderBy(u => u.Username).ToListAsync();
    }

    public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
    {
        var allowedRoles = new[] { "Customer", "StoreStaff", "Admin" };
        var role = allowedRoles.FirstOrDefault(r =>
            string.Equals(r, newRole?.Trim(), StringComparison.OrdinalIgnoreCase));
        if (role is null) return false;

        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var user = await ctx.AppUsers.FindAsync(userId);
        if (user is null) return false;
        user.Role = role;
        await ctx.SaveChangesAsync();
        return true;
    }
}
