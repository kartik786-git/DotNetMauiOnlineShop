namespace ShopApp.Models;

public class Order
{
    public int Id { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }
    
    public List<OrderItem> Items { get; set; } = new();

    public int ItemsCount => Items?.Count ?? 0;
    
    public string OrderDateDisplay => OrderDate.ToString("g");
    public string TotalAmountDisplay => TotalAmount.ToString("C");
}
