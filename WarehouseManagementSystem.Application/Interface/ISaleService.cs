using WarehouseManagementSystem.Application.DTOs;

namespace WarehouseManagementSystem.Application.Interface;

public interface ISaleService
{
    Task<SaleDto> CreateSaleAsync(SaleCreateDto saleCreateDto);

    Task<List<SaleDto>> GetAllSalesAsync();

    Task<List<SaleDto>> GetUserSalesAsync(string userId);

    Task<SaleDto> GetSaleByIdAsync(int id);
}
