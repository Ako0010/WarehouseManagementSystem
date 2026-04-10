
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Infrastructure.Data;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class CategoryService : ICategoryService
{
    private readonly WarehouseManagementDBContext _context;
    private readonly IMapper _mapper;

    public CategoryService(WarehouseManagementDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        var category = _mapper.Map<Category>(dto);

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<bool> DeleteCategoriesAsync(int id) 
    {
        var category = await _context.Categories.FindAsync(id);

        if (category is null)
            return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;

    }
}
