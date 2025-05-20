using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartItem> AddToCartAsync(int userId, CartItemDTO dto);
        Task<IEnumerable<CartItem>> GetUserCartAsync(int userId);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
    }
}
