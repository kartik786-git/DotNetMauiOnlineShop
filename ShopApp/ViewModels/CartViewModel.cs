using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Models;
using ShopApp.Services;
using System.Collections.ObjectModel;

namespace ShopApp.ViewModels;

public partial class CartViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private const string SessionId = "user-session-123"; // TODO: Manage session properly

    public ObservableCollection<CartItem> CartItems { get; } = new();

    [ObservableProperty]
    decimal totalAmount;

    public CartViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Shopping Cart";
    }

    [RelayCommand]
    async Task LoadCartAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var items = await _apiService.GetCartAsync(SessionId);
            CartItems.Clear();
            decimal total = 0;
            foreach (var item in items)
            {
                CartItems.Add(item);
                total += item.TotalPrice;
            }
            TotalAmount = total;
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

    [RelayCommand]
    async Task CheckoutAsync()
    {
        if (CartItems.Count == 0)
        {
            await Shell.Current.DisplayAlert("Info", "Cart is empty", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            // Ask for email or get from user profile
            string email = await Shell.Current.DisplayPromptAsync("Checkout", "Enter your email:");
            if (string.IsNullOrWhiteSpace(email)) return;

            bool success = await _apiService.CreateOrderAsync(SessionId, email);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Order placed successfully!", "OK");
                CartItems.Clear();
                TotalAmount = 0;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to place order", "OK");
            }
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
