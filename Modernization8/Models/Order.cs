namespace Modernization8.Models;

public class Order
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal TotalPrice => Quantity * ProductPrice;
}