using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDTO dto,int userId);

        Task<List<Order>> GetAllOrdersAsync();

        Task<Order?> GetOrderByIdAsync(int id);

        Task<List<OrderResponseDTO>>GetOrdersByUserIdAsync(int userId);
    }
}
