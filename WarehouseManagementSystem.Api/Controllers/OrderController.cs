using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Infrastructure.Service;

namespace WarehouseManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly WarehouseManagementDBContext _context;
    private string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public OrderController(IOrderService service,WarehouseManagementDBContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        await _service.CreateOrderAsync(dto,UserId);
        return Ok();
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var orders = await _context.Orders
            .Include(x => x.Product)
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                x.Id,
                Product = x.Product.Name,
                x.Quantity,
                x.CustomerName,
                Status = x.Status.ToString(),
                x.CreatedAt,
                IsReadyForProcessing = x.IsReadyForProcessing ? "Yes" : "No"
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost("complete/{id}")]
    public async Task<IActionResult> Complete(int id)
    {
        await _service.CompleteOrderAsync(id);
        return Ok();
    }

    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _service.CancelOrderAsync(id);
        return Ok();
    }

    [HttpPost("processing/{id}")]
    public async Task<IActionResult> Processing(int id)
    {
        await _service.ProcessingOrderAsync(id);
        return Ok();
    }

    [HttpPost("finish-processing/{id}")]
    public async Task<IActionResult> FinishProcessing(int id)
    {
        await _service.FinshProcessingAsync(id);
        return Ok();
    }
}
