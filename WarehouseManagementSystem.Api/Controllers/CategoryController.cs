using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;

namespace WarehouseManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("add")]
    public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
    {
        var result = await _categoryService.CreateCategoryAsync(dto);

        return Ok(result);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoriesAsync(id);

        if (!result)
            return BadRequest();

        return Ok(result);
    }
}
