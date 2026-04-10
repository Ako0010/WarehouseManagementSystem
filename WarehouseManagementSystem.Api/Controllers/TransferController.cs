using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Application.DTOs;

namespace WarehouseManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly WarehouseManagementDBContext _context;

    public TransferController(WarehouseManagementDBContext context)
    {
        _context = context;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Transfer(TransferDto transferDto)
    {
        var product = await _context.Products.FindAsync(transferDto.ProductId);

        if (product is null)
            return NotFound("Product not found");

        if (product.LocationId != transferDto.FromId)
            return BadRequest("Product is not in this location");

        if (product.Quantity < transferDto.Quantity)
            return BadRequest("Not enough stock");

        product.Quantity -= transferDto.Quantity;
        UpdateProductStatus(product);

        var targetProduct = await _context.Products
            .FirstOrDefaultAsync(x =>
            x.Name == product.Name &&
            x.LocationId == transferDto.ToId
            );

        if (targetProduct is null)
            {
            targetProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = transferDto.Quantity,
                LocationId = transferDto.ToId,
                CategoryId = product.CategoryId,
                StockLimit = product.StockLimit,
                CreatedAt = DateTime.UtcNow
            };

            UpdateProductStatus(targetProduct);
            _context.Products.Add(targetProduct);
            await _context.SaveChangesAsync();
        }
        else
        {
            targetProduct.Quantity += transferDto.Quantity;
            UpdateProductStatus(targetProduct);
        }

        _context.StockMovements.Add(new StockMovement
        {
            ProductId = targetProduct.Id,
            Quantity = transferDto.Quantity,
            Type = "Transfer",
            FromLocationId = transferDto.FromId,
            ToLocationId = transferDto.ToId,
            Date = DateTime.UtcNow
        });

        _context.Transfers.Add(new Transfer
        {
            ProductId = transferDto.ProductId,
            FromLocationId = transferDto.FromId,
            ToLocationId = transferDto.ToId,
            Quantity = transferDto.Quantity
        });

        await _context.SaveChangesAsync();

        return Ok("Transferred successfully");
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
}