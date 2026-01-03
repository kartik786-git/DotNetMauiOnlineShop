using CommunityToolkit.Mvvm.ComponentModel;

namespace ShopApp.Models;

public partial class CartItem : ObservableObject
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    private int _quantity;

    public decimal TotalPrice => ProductPrice * Quantity;
}
