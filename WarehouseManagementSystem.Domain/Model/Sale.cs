namespace WarehouseManagementSystem.Domain.Model;

public class Sale
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
    
    public string UserId { get; set; }

    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
}