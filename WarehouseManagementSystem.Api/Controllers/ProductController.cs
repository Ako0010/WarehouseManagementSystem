using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Common;

namespace WarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(products, "Products returned successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(ApiResponse<ProductDto>.SuccessResponse(product,"Product returned successfully"));
    }

    [HttpPost("reduce-stock")]
    public async Task<ActionResult> ReduceStock(int productId, int quantity)
    {
        var result = await _productService.ReduceStockAsync(productId, quantity);

        if (!result)
            return NotFound();

        return Ok(ApiResponse<ProductDto>.SuccessResponse("Product stock updated successfully"));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("add")]
    public async Task<ActionResult> Create([FromBody] ProductCreateDto productCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdProduct = await _productService.AddProductAsync(productCreateDto);
        return CreatedAtAction(
            nameof(GetById),
            new { createdProduct.Id },
            ApiResponse<ProductDto>.SuccessResponse(createdProduct, "Product created successfully"));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("update/{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedProduct = await _productService.UpdateProductAsync(id,productUpdateDto);
        return Ok(ApiResponse<ProductDto>.SuccessResponse(updatedProduct, "Product Updated successfully"));

    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (!result)
            return NotFound();

        return Ok(result);
    }
}
