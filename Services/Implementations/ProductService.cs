using AutoMapper;
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;
using EshoppingZoneAPI.Repositories.Interfaces;
using EshoppingZoneAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZoneAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        // Get product by ID
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        // Add a new product
        public async Task<Product> AddProductAsync(ProductDTO dto)
        {
            // Check if the category exists before mapping
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {dto.CategoryId} does not exist.");
            }

            // Map DTO to Product entity
            var product = _mapper.Map<Product>(dto);
            product.Category = category;  // Assign category to product

            // Add product to repository
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        // Update an existing product
        public async Task<bool> UpdateProductAsync(int id, ProductDTO dto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return false;

            // Check if the category exists before updating
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                throw new Exception($"Category with ID {dto.CategoryId} does not exist.");
            }

            // Map the updated fields to the existing product
            _mapper.Map(dto, existingProduct);
            existingProduct.Category = category;  // Ensure category is updated

            // Update product in repository
            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        // Delete a product by ID
        public async Task<bool> DeleteProductAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return false;

            // Delete the product from the repository
            _productRepository.Delete(existingProduct);
            await _productRepository.SaveChangesAsync();

            return true;
        }
    }
}
