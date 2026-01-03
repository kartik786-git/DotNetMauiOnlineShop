using System.Net.Http.Json;
using System.Text.Json;
using ShopApp.Models;

namespace ShopApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient; 
    private readonly JsonSerializerOptions _serializerOptions;
    // For Android Emulator, localhost is 10.0.2.2. For Windows, it's localhost.
    private const string BaseUrl = "https://shopnowapi.runasp.net"; 

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        try 
        {
            var response = await _httpClient.GetAsync("/api/categories");
            if (response.IsSuccessStatusCode)
            {
               return await response.Content.ReadFromJsonAsync<List<Category>>(_serializerOptions) ?? new();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching categories: {ex.Message}");
        }
        return new List<Category>();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>(_serializerOptions);
                if (products != null)
                {
                    foreach (var p in products)
                    {
                        p.ImageUrl = GetImageForProduct(p.Name, p.CategoryName);
                    }
                    return products;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching products: {ex.Message}");
        }
        return new List<Product>();
    }

    private string GetImageForProduct(string? productName, string? categoryName)
    {
        var name = productName?.ToLower() ?? "";
        var cat = categoryName?.ToLower() ?? "";

        // Electronics / Gadgets
        if (name.Contains("iphone") || name.Contains("phone") || name.Contains("mobile")) return "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&q=80";
        if (name.Contains("samsung") || name.Contains("galaxy") || name.Contains("android")) return "https://images.unsplash.com/photo-1610945415295-d9bbf067e59c?w=500&q=80";
        if (name.Contains("macbook") || name.Contains("laptop") || name.Contains("notebook")) return "https://images.unsplash.com/photo-1517336714731-489689fd1ca4?w=500&q=80";
        if (name.Contains("watch") || name.Contains("smartwatch")) return "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=500&q=80";
        if (name.Contains("headphone") || name.Contains("audio") || name.Contains("earbud") || name.Contains("sony")) return "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80";
        if (name.Contains("camera") || name.Contains("dslr") || name.Contains("canon") || name.Contains("nikon")) return "https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=500&q=80";
        if (name.Contains("tv") || name.Contains("television") || name.Contains("monitor")) return "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?w=500&q=80";
        
        // Fashion / Clothing
        if (name.Contains("shoe") || name.Contains("sneaker") || name.Contains("nike") || name.Contains("adidas")) return "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500&q=80";
        if (name.Contains("shirt") || name.Contains("t-shirt") || name.Contains("top") || name.Contains("polo")) return "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500&q=80";
        if (name.Contains("jean") || name.Contains("denim") || name.Contains("pant")) return "https://images.unsplash.com/photo-1542272454315-4c01d7abdf4a?w=500&q=80";
        if (name.Contains("jacket") || name.Contains("coat") || name.Contains("hoodie")) return "https://images.unsplash.com/photo-1551028919-ac66993a652e?w=500&q=80";
        if (name.Contains("bag") || name.Contains("backpack") || name.Contains("purse")) return "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80";
        
        // Fallback by Category
        if (cat.Contains("electronic") || cat.Contains("gadget")) return "https://images.unsplash.com/photo-1498049860654-af1a5c5668ba?w=500&q=80";
        if (cat.Contains("fashion") || cat.Contains("cloth") || cat.Contains("wear")) return "https://images.unsplash.com/photo-1445205170230-053b83016050?w=500&q=80";
        if (cat.Contains("beauty") || cat.Contains("health")) return "https://images.unsplash.com/photo-1522335789203-abd315f08f7b?w=500&q=80";
        if (cat.Contains("home") || cat.Contains("living")) return "https://images.unsplash.com/photo-1484101403633-562f891dc89a?w=500&q=80";

        // Generic fallback with a reliable image
        return "https://images.unsplash.com/photo-1556742049-0cfed4f7a07d?w=500&q=80";
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"/api/products/{id}", _serializerOptions);
    }

    public async Task<List<CartItem>> GetCartAsync(string sessionId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<CartItem>>($"/api/cart/{sessionId}", _serializerOptions) ?? new();
        }
        catch
        {
            return new List<CartItem>();
        }
    }

    public async Task AddToCartAsync(string sessionId, int productId, int quantity)
    {
        var item = new { productId, quantity };
        await _httpClient.PostAsJsonAsync($"/api/cart/{sessionId}", item, _serializerOptions);
    }

    public async Task<bool> CreateOrderAsync(string sessionId, string email)
    {
        var orderRequest = new { sessionId, customerEmail = email };
        var response = await _httpClient.PostAsJsonAsync("/api/orders", orderRequest, _serializerOptions);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/orders");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<Order>>(content, _serializerOptions) ?? new();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetOrdersAsync Error: {ex.Message}");
        }
        return new List<Order>();
    }
}
