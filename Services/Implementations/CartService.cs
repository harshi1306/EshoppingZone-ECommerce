using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;
using EshoppingZoneAPI.Repositories.Interfaces;
using EshoppingZoneAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZoneAPI.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartItem> _cartRepo;
        private readonly IRepository<Product> _productRepo;

        public CartService(IRepository<CartItem> cartRepo, IRepository<Product> productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<CartItem> AddToCartAsync(int userId, CartItemDTO dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId);
            if (product == null || product.Quantity < dto.Quantity)
                throw new Exception("Invalid product or not enough stock.");

            var cartItem = new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UserId = userId
            };

            await _cartRepo.AddAsync(cartItem);
            await _cartRepo.SaveChangesAsync();

            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetUserCartAsync(int userId)
        {
            return await _cartRepo.FindAsync(c => c.UserId == userId);
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cartItems = await _cartRepo.FindAsync(c => c.UserId == userId && c.CartItemId == cartItemId);
            var cartItem = cartItems.FirstOrDefault();
            if (cartItem == null) return false;
            await _cartRepo.DeleteAsync(cartItem); // ✅ Correct

            await _cartRepo.SaveChangesAsync();
            return true;
        }
    }
}
