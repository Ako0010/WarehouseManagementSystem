

namespace WarehouseManagementSystem.Domain.Model;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; } 
    public int? FromLocationId { get; set; }
    public Location FromLocation { get; set; }
    public int? ToLocationId { get; set; }
    public Location ToLocation { get; set; }
    public string? CustomerName { get; set; }
    public DateTime Date { get; set; }
    public string? UserId { get; set; }
}
