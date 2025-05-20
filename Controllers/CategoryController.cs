using Microsoft.AspNetCore.Mvc;
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;
using EshoppingZoneAPI.Repositories.Interfaces;

namespace EshoppingZoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepo;

        public CategoryController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
