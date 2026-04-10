

namespace WarehouseManagementSystem.Domain.Model;

public class Transfer
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int FromLocationId { get; set; }
    public Location FromLocation { get; set; }
    public int ToLocationId { get; set; }
    public Location ToLocation { get; set; }
    public int Quantity { get; set; }

}
