using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Services.Interfaces;

namespace EshoppingZoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ✅ Allow only Merchants to add products
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO dto)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Merchant")
                return Forbid("Only merchants can add products.");

            var result = await _productService.AddProductAsync(dto);
            return Ok(result);
        }

        // ✅ All users can view products
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
