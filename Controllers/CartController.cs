using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Services.Interfaces;

namespace EshoppingZoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO dto)
        {
            var result = await _cartService.AddToCartAsync(GetUserId(), dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartService.GetUserCartAsync(GetUserId());
            return Ok(cart);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var removed = await _cartService.RemoveFromCartAsync(GetUserId(), cartItemId);
            if (!removed) return NotFound();
            return Ok("Item removed from cart");
        }
    }
}
