using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Application.Interface;

public interface IOrderService
{
    Task CreateOrderAsync(OrderCreateDto dto, string userId);
    Task<List<OrderDto>> GetAllAsync();
    Task CompleteOrderAsync(int orderId);
    Task ProcessingOrderAsync(int orderId);
    Task FinshProcessingAsync(int orderId);
    Task CancelOrderAsync(int orderId);
}