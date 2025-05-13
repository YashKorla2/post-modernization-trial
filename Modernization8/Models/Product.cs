namespace Modernization8.Models;
public class Product
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public required string ProductCategory { get; set; }
    public double ProductRating { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductAvailableQuantity { get; set; }
    public decimal ProductDiscount { get; set; }
}