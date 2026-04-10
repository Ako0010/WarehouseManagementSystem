
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Infrastructure.Data;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class OrderService : IOrderService
{
    private readonly WarehouseManagementDBContext _context;

    public OrderService(WarehouseManagementDBContext context)
    {
        _context = context;
    }
    public async Task CreateOrderAsync(OrderCreateDto dto, string userId)
    {

        var order = new Order
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            CustomerName = dto.CustomerName,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

    }
    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Product)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                Product = o.Product.Name,
                Quantity = o.Quantity,
                CustomerName = o.CustomerName,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                IsReadyForProcessing = o.IsReadyForProcessing ? "Yes" : "No"
            })
            .ToListAsync();

        return orders;
    }
    public async Task CompleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order is null) throw new Exception("Order not found");


        var product = await _context.Products.FindAsync(order.ProductId);
        
        if (product.Quantity < order.Quantity) throw new Exception("Not enough stock");

        product.Quantity -= order.Quantity;

        UpdateProductStatus(product);

        order.Status = OrderStatus.Completed;

        _context.StockMovements.Add(new StockMovement
        {
            ProductId = product.Id,
            Quantity = -order.Quantity,
            Type = "Completed",
            FromLocationId = product.LocationId,
            CustomerName = order.CustomerName,
            Date = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task CancelOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order is null) throw new Exception("Order not found");

        if (order.Status != OrderStatus.Pending) throw new Exception("Only pending orders can be cancelled");

        order.Status = OrderStatus.Cancelled;

        await _context.SaveChangesAsync();
    }

    private void UpdateProductStatus(Product product)
    {
        if (product.Quantity == 0)
            product.Status = ProductStatus.OutOfStock;

        else if (product.Quantity <= product.StockLimit)
            product.Status = ProductStatus.LowStock;

        else
            product.Status = ProductStatus.Active;
    }

    public async Task ProcessingOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order is null) throw new Exception("Order not found");

        if (order.Status != OrderStatus.Pending) throw new Exception("Only pending orders can be processed");

        order.Status = OrderStatus.Processing;

        _context.StockMovements.Add(new StockMovement
        {
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Type = "Processing",
            FromLocationId = null,
            CustomerName = order.CustomerName,
            Date = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task FinshProcessingAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order is null) throw new Exception("Order not found");

        if (order.Status != OrderStatus.Processing) throw new Exception("Only processing orders can be finished");

        order.IsReadyForProcessing = true;

        await _context.SaveChangesAsync();
    }
}
