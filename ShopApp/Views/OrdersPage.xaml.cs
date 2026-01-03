using ShopApp.ViewModels;

namespace ShopApp.Views;

public partial class OrdersPage : ContentPage
{
    private readonly OrdersViewModel _viewModel;

	public OrdersPage(OrdersViewModel viewModel)
	{
        try 
        {
		    InitializeComponent();
		    BindingContext = _viewModel = viewModel;
        }
        catch(Exception ex)
        {
             // Log error if needed
             System.Diagnostics.Debug.WriteLine($"OrdersPage Constructor Failed: {ex}");
        }
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModel.LoadOrdersCommand.ExecuteAsync(null);
        }
        catch
        {
            // Ignore implicit load errors
        }
    }
}
