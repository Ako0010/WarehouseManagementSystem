
namespace WarehouseManagementSystem.Application.DTOs;

public class OrderCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }

}

public class OrderDto
{
    public int Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string IsReadyForProcessing { get; set; }
}