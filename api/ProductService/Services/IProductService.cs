using Productservice.DTO;
using Productservice.Models;

namespace Productservice.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();

        Task<List<Product>> GetAllAdminProductsAsync();

        Task<Product?> GetProductByIdAsync(int id);

        Task<Product> CreateProductAsync(ProductDTO dto);

        Task<bool> UpdateProductAsync(int id, ProductDTO dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
