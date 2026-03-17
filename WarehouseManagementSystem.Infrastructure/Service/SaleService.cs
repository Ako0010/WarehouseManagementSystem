using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Application.Interface;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class SaleService : ISaleService
{
    private readonly WarehouseManagementDBContext _context;
    private readonly IMapper _mapper;

    public SaleService(WarehouseManagementDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SaleDto> CreateSaleAsync(SaleCreateDto saleCreateDto, string userId)
    {
        var product = await _context.Products.FindAsync(saleCreateDto.ProductId);

        if (product is null)
            return null;

        if (product.Quantity < saleCreateDto.Quantity)
            throw new Exception("Not enough stock");

        product.Quantity -= saleCreateDto.Quantity;

        var sale = _mapper.Map<Sale>(saleCreateDto);

        sale.UserId = userId;
        sale.TotalPrice = product.Price * saleCreateDto.Quantity;
        sale.SaleDate = DateTime.UtcNow;

        _context.Sales.Add(sale);

        await _context.SaveChangesAsync();

        return _mapper.Map<SaleDto>(sale);
    }

    public async Task<List<SaleDto>> GetAllSalesAsync()
    {
        var sales = await _context.Sales
            .Include(x => x.Product)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<SaleDto>>(sales);
    }

    public async Task<SaleDto> GetSaleByIdAsync(int id)
    {
        var sale = await _context.Sales
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (sale is null)
            return null;

        return _mapper.Map<SaleDto>(sale);
    }

    public async Task<List<SaleDto>> GetUserSalesAsync(string userId)
    {
        var sales = await _context.Sales
            .Where(x => x.UserId == userId)
            .Include(x => x.Product)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<SaleDto>>(sales);
    }
}
