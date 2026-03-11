using WarehouseManagementSystem.DTOs;

namespace WarehouseManagementSystem.Service.Interface;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<ProductDto> AddProductAsync(ProductCreateDto productCreateDto);
    Task<ProductDto> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> ReduceStockAsync(int productId, int quantity);
}
