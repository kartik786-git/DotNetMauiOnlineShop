using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Models;
using ShopApp.Services;

namespace ShopApp.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class ProductDetailsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    public ProductDetailsViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Product Details";
    }

    [ObservableProperty]
    Product product;

    [ObservableProperty]
    int quantity = 1;

    [RelayCommand]
    void IncreaseQuantity()
    {
        if (Quantity < 99) Quantity++;
    }

    [RelayCommand]
    void DecreaseQuantity()
    {
        if (Quantity > 1) Quantity--;
    }

    [RelayCommand]
    async Task AddToCartAsync()
    {
        if (IsBusy || Product == null) return;

        try
        {
            IsBusy = true;
            // Hardcoded session ID for demo purposes
            string sessionId = "user-session-123"; 
            await _apiService.AddToCartAsync(sessionId, Product.Id, Quantity);
            await Shell.Current.DisplayAlert("Success", "Added to cart", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
