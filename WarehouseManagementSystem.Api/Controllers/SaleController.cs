using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseManagementSystem.Domain.Common;
using WarehouseManagementSystem.Application .DTOs;
using WarehouseManagementSystem.Application.Interface;

namespace WarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

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

    [HttpPost]

    public async Task<ActionResult> Create([FromBody] SaleCreateDto saleCreateDto)
    {
        var createdSale = await _saleService.CreateSaleAsync(saleCreateDto);

        return CreatedAtAction(
            nameof(GetById),
            new { createdSale.Id },
            ApiResponse<SaleDto>.SuccessResponse(createdSale, "Sale created successfully"));
    }

    [HttpGet("my-sales")]
    public async Task<ActionResult<List<SaleDto>>> GetMySales()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var sales = await _saleService.GetUserSalesAsync(userId);

        return Ok(sales);
    }

}
