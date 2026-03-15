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

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
    {
        var result = await _categoryService.CreateCategoryAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        return Ok(result);
    }
}
