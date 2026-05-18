using Productservice.Models;

namespace Productservice.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetAllAdminAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);

        Task<Product?> UpdateAsync(Product product);

        Task<bool> DeleteAsync(int id);
    }
}
