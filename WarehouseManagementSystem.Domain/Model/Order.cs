using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Domain.Model;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Product Product { get; set; }
    public string UserId { get; set; }
    public bool IsReadyForProcessing { get; set; } = false;

}

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}