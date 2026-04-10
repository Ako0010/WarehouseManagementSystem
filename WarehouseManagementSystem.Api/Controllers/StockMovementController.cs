using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Data;

namespace WarehouseManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockMovementController : ControllerBase
{
    private readonly WarehouseManagementDBContext _context;

    public StockMovementController(WarehouseManagementDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movements = await _context.StockMovements
            .Include(x => x.Product)
            .Include(x => x.FromLocation)
            .Include(x => x.ToLocation)
            .OrderByDescending(x => x.Date)
            .Select(x => new
            {
                x.Id,
                Product = x.Product.Name,
                x.Quantity,
                x.Type,
                From = x.Type == "Add" ? "Supplier" : x.FromLocation != null ? x.FromLocation.Code : "Worker",
                To = (x.Type == "Completed" || x.Type == "Processing") ? x.CustomerName : (x.Type == "Add" || x.Type == "Transfer")  ? x.ToLocation.Code : "-",
                x.Date
            })
            .ToListAsync();

        return Ok(movements);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var movements = await _context.StockMovements
            .Include(x => x.Product)
            .Include(x => x.FromLocation)
            .Include(x => x.ToLocation)
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.Date)
            .Select(x => new
            {
                x.Id,
                x.ProductId,
                x.Quantity,
                x.Type,
                x.Date,
                From = x.Type == "Add" ? "Supplier" : x.FromLocation != null ? x.FromLocation.Code : "-",
                To = (x.Type == "Completed" || x.Type == "Processing") ? x.CustomerName : (x.Type == "Add" || x.Type == "Transfer") ? x.ToLocation.Code : "-",
            })
            .ToListAsync();

        return Ok(movements);
    }
}