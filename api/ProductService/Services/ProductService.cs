using Productservice.DTO;
using Productservice.Models;
using Productservice.Repository;

namespace Productservice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        // User Products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _repo.GetAllAsync();
        }

        // Admin Products
        public async Task<List<Product>> GetAllAdminProductsAsync()
        {
            return await _repo.GetAllAdminAsync();
        }

        // Product By Id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // Create Product
        public async Task<Product> CreateProductAsync(ProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                Gender=dto.Gender,
                IsActive = true,
                ProductImages = dto.ProductImages
                    .Select(x => new ProductImage
                    {
                        ImageUrl = x.ImageUrl
                    }).ToList(),
                    // Sizes
                ProductSizes =
                    dto.ProductSizes
                    .Select(x => new ProductSize
                    {
                        SizeValue = x.SizeValue,
                        Stock = x.Stock
                    }).ToList()
            };

            return await _repo.AddAsync(product);
        }

        // Update Product
        public async Task<bool> UpdateProductAsync(
            int id,
            ProductDTO dto)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                return false;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;
            product.Gender = dto.Gender;
            // Update Multiple Images
            product.ProductImages =
                dto.ProductImages
                .Select(x => new ProductImage
                {
                    ProductId = id,
                    ImageUrl = x.ImageUrl
                }).ToList();
            // Update Sizes
            product.ProductSizes =
                dto.ProductSizes
                .Select(x => new ProductSize
                {
                    ProductId = id,
                    SizeValue = x.SizeValue,
                    Stock = x.Stock
                }).ToList();

            await _repo.UpdateAsync(product);

            return true;
        }

        // Soft Delete Product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                return false;

            // Soft Delete
            product.IsActive = false;

            await _repo.UpdateAsync(product);

            return true;
        }
    }
}
