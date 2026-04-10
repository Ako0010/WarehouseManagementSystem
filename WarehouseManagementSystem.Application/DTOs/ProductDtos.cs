namespace WarehouseManagementSystem.Application.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int StockLimit { get; set; }
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
}

public class ProductUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int StockLimit { get; set; }
    public int CategoryId { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public string CategoryName { get; set; }
    public string LocationCode { get; set; }
    public int StockLimit { get; set; }
}
