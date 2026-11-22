using BookStoresBlazor.Data.Models;
using BookStoresBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresBlazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        var result = orders.Select(o => new
        {
            o.OrderId,
            o.OrderNumber,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.PaymentMethod,
            Customer = o.Customer?.Username,
            CustomerEmail = o.CustomerEmail ?? o.Customer?.EmailAddress,
            o.ShippingName,
            o.ShippingAddress,
            o.ShippingCity,
            o.ShippingPostCode,
            ItemCount = o.Items.Sum(i => i.Quantity)
        });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound(new { message = $"Order #{id} not found." });

        return Ok(new
        {
            order.OrderId,
            order.OrderNumber,
            order.OrderDate,
            order.Status,
            order.TotalAmount,
            order.PaymentMethod,
            order.ShippingName,
            order.CustomerEmail,
            order.CustomerPhone,
            order.ShippingAddress,
            order.ShippingCity,
            order.ShippingPostCode,
            Customer = order.Customer?.Username,
            Items = order.Items.Select(i => new
            {
                i.OrderItemId,
                i.BookId,
                i.BookTitle,
                i.Quantity,
                i.UnitPrice,
                i.LineTotal
            })
        });
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<IActionResult> GetCustomerOrders(int customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        var result = orders.Select(o => new
        {
            o.OrderId,
            o.OrderNumber,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.PaymentMethod,
            o.ShippingName,
            o.ShippingAddress,
            o.ShippingCity,
            o.ShippingPostCode,
            Items = o.Items.Select(i => new
            {
                i.BookId,
                i.BookTitle,
                i.Quantity,
                i.UnitPrice,
                i.LineTotal
            })
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto request)
    {
        if (request.CustomerId <= 0)
            return BadRequest(new { message = "Valid CustomerId is required." });

        if (string.IsNullOrWhiteSpace(request.ShippingName) || string.IsNullOrWhiteSpace(request.ShippingAddress) || string.IsNullOrWhiteSpace(request.ShippingCity))
            return BadRequest(new { message = "Shipping name, address, and city are required." });

        if (request.Items == null || !request.Items.Any())
            return BadRequest(new { message = "Order must contain at least one item." });

        try
        {
            var cartItems = request.Items.Select(i => new CartItem
            {
                BookId = i.BookId,
                Title = i.Title,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            });

            var order = await _orderService.PlaceOrderAsync(
                request.CustomerId,
                request.ShippingName,
                request.ShippingAddress,
                request.ShippingCity,
                request.ShippingPostCode ?? "",
                request.CustomerEmail ?? "",
                request.CustomerPhone ?? "",
                request.PaymentMethod ?? "Cash on Delivery",
                cartItems);

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, new
            {
                order.OrderId,
                order.OrderNumber,
                order.Status,
                order.TotalAmount,
                order.OrderDate
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An internal server error occurred while processing the order." });
        }
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
            return BadRequest(new { message = "Status string is required." });

        var (success, message) = await _orderService.UpdateOrderStatusAsync(id, request.Status);
        if (!success)
            return BadRequest(new { message });

        return Ok(new { message });
    }

    public record CreateOrderDto(
        int CustomerId,
        string ShippingName,
        string ShippingAddress,
        string ShippingCity,
        string? ShippingPostCode,
        string? CustomerEmail,
        string? CustomerPhone,
        string? PaymentMethod,
        List<CreateOrderItemDto> Items);

    public record CreateOrderItemDto(string BookId, string Title, decimal UnitPrice, int Quantity);
    public record UpdateStatusDto(string Status);
}
