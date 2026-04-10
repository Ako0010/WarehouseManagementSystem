

namespace WarehouseManagementSystem.Domain.Model;

public class Location
{
    public int Id { get; set; }
    public string Code { get; set; }
    public List<Product>? Products { get; set; }
}
