using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseManagementSystem.Application .DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Common;

namespace WarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;
    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("get-all")]
    public async Task<ActionResult> GetAll()
    {
        var sales = await _saleService.GetAllSalesAsync();
        return Ok(ApiResponse<IEnumerable<SaleDto>>.SuccessResponse(sales, "Sales returned successfully"));
    }

    [HttpGet("{id:int}")]

    public async Task<ActionResult> GetById(int id)
    {
        var sale = await _saleService.GetSaleByIdAsync(id);
        return Ok(ApiResponse<SaleDto>.SuccessResponse(sale, "Sale returned successfully"));
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPost]

    public async Task<ActionResult> Create([FromBody] SaleCreateDto saleCreateDto)
    {
        var createdSale = await _saleService.CreateSaleAsync(saleCreateDto,UserId);

        return CreatedAtAction(
            nameof(GetById),
            new { createdSale.Id },
            ApiResponse<SaleDto>.SuccessResponse(createdSale, "Sale created successfully"));
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("my-sales")]
    public async Task<ActionResult<List<SaleDto>>> GetMySales()
    {
        var sales = await _saleService.GetUserSalesAsync(UserId);

        return Ok(sales);
    }

}
