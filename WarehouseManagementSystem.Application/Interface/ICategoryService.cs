namespace WarehouseManagementSystem.Application.Interface;

using WarehouseManagementSystem.Application.DTOs;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);

    Task<List<CategoryDto>> GetAllCategoriesAsync();

    Task<bool> DeleteCategoriesAsync(int id);
}
