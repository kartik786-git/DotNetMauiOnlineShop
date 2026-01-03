using CommunityToolkit.Mvvm.Input;
using ShopApp.Models;
using ShopApp.Services;
using System.Collections.ObjectModel;

namespace ShopApp.ViewModels;

public partial class OrdersViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    public ObservableCollection<Order> Orders { get; } = new();

    public OrdersViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "My Orders";
    }

    [RelayCommand]
    async Task LoadOrdersAsync()
    {
        if (IsBusy) return;
        
        try
        {
            IsBusy = true;
            var orders = await _apiService.GetOrdersAsync();
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Orders.Clear();
                foreach (var order in orders) Orders.Add(order);
            });
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
