using ShopApp.Views;

namespace ShopApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
    }
}
