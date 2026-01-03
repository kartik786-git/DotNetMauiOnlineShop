using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Models;
using ShopApp.Services;
using System.Collections.ObjectModel;

namespace ShopApp.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<Product> Products { get; } = new();

    public HomeViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Shop Home";
        // Trigger load in OnNavigatedTo or just call manually if needed
    }

    [RelayCommand]
    async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            
            var categories = await _apiService.GetCategoriesAsync();
            var products = await _apiService.GetProductsAsync();

            Categories.Clear();
            foreach (var category in categories)
                Categories.Add(category);

            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }
        catch (Exception ex)
        {
            // Ideally use an injected IDialogService, but Shell.Current is quick for now
            await Shell.Current.DisplayAlert("Error", $"Unable to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task GoToProductDetails(Product product)
    {
        if (product is null) return;

        // Navigation will be setup later
        await Shell.Current.GoToAsync($"ProductDetailsPage", true, new Dictionary<string, object>
        {
            {"Product", product }
        });
    }
}
