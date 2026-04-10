

namespace WarehouseManagementSystem.Application.DTOs;

public class TransferDto
{
    public int ProductId { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public int Quantity { get; set; }
}