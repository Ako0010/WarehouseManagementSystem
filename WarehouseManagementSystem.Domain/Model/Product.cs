namespace WarehouseManagementSystem.Domain.Model;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public int StockLimit { get; set; }
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime CreatedAt { get; set; }
}

public enum ProductStatus
{
    Active,
    LowStock,
    OutOfStock,
    Discontinued
}