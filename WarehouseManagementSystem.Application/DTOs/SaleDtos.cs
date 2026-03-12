namespace WarehouseManagementSystem.Application.DTOs;

public class SaleCreateDto
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}


public class SaleDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime SaleDate { get; set; }
}