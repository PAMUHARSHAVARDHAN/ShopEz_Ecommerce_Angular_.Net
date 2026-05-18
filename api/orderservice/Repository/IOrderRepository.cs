using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);

        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    }
}
