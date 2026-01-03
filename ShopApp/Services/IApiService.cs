using ShopApp.Models;

namespace ShopApp.Services;

public interface IApiService
{
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Product>> GetProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<List<CartItem>> GetCartAsync(string sessionId);
    Task AddToCartAsync(string sessionId, int productId, int quantity);
    Task<bool> CreateOrderAsync(string sessionId, string email);
    Task<List<Order>> GetOrdersAsync();
}
