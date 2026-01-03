namespace ShopApp.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtTime { get; set; }
    public decimal TotalPrice { get; set; }
}
