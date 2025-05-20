using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(ProductDTO dto);
        Task<bool> UpdateProductAsync(int id, ProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
    }
}
