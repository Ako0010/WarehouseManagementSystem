using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Application.Interface;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class ProductService : IProductService
{
    private readonly WarehouseManagementDBContext _context;
    private readonly IMapper _mapper;

    public ProductService(WarehouseManagementDBContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<ProductDto> AddProductAsync(ProductCreateDto productCreateDto)
    {
        var product = _mapper.Map<Product>(productCreateDto);

        product.CreatedAt = DateTime.UtcNow;

        UpdateProductStatus(product);

        _context.Products.Add(product);

        _context.StockMovements.Add(new StockMovement
        {
            Product = product,
            Quantity = product.Quantity,
            Type = "Add",
            FromLocationId = null,
            ToLocationId = product.LocationId,
            Date = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _context.Products
                             .Include(p => p.Category)
                             .Include(p => p.Location)
                             .AsNoTracking()
                             .ToListAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Location)
                            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> ReduceStockAsync(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            return false;

        if (product.Quantity < quantity)
            return false;

        product.Quantity -= quantity;

        UpdateProductStatus(product);

        _context.StockMovements.Add(new StockMovement
        {
            Product = product,
            Quantity = -quantity,
            Type = "Sale",
            FromLocationId = product.LocationId,
            ToLocationId = null,
            Date = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ProductDto> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            return null;

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == productUpdateDto.CategoryId);

        if (!categoryExists)
            throw new Exception("Category not found");


        _mapper.Map(productUpdateDto,product);

        UpdateProductStatus(product);

        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
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
