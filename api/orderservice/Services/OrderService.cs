using OrderService.DTOs;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly HttpClient _httpClient;

        public OrderService(IOrderRepository repo, HttpClient httpClient)
        {
            _repo = repo;
            _httpClient = httpClient;
        }

        // Create Order
        public async Task<Order>
            CreateOrderAsync(CreateOrderDTO dto, int userId)
        {
            // Reduce Product Stock
            foreach (var item in dto.Items)
            {
                var response =
                    await _httpClient.PutAsJsonAsync(
                        $"https://localhost:7125/api/Product/decrease-stock/{item.ProductId}",
                        item.Quantity);

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        await response.Content.ReadAsStringAsync();

                    throw new Exception(error);
                }
            }

            var orderItems = dto.Items.Select(i =>
                new OrderItem
                {
                    ProductId = i.ProductId,

                    ProductName = i.ProductName,

                   

                    Quantity = i.Quantity,

                    Price = i.Price,
                     SelectedSize = i.SelectedSize
                }).ToList();

            var order = new Order
            {
                UserId = userId,

                FullName = dto.FullName,

                Email = dto.Email,

                Address = dto.Address,

                TotalAmount = dto.TotalAmount,

                OrderDate = DateTime.UtcNow,

                OrderItems = orderItems
            };

            return await _repo.AddAsync(order);
        }

        // Get All Orders
        public async Task<List<Order>>
            GetAllOrdersAsync()
        {
            return await _repo.GetAllAsync();
        }

        // Get Order By Id
        public async Task<Order?>
            GetOrderByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // User Orders
        public async Task<List<OrderResponseDTO>>
            GetOrdersByUserIdAsync(int userId)
        {
            var orders =
                await _repo.GetOrdersByUserIdAsync(userId);

            return orders.Select(o =>
                new OrderResponseDTO
                {
                    OrderId = o.OrderId,

                    OrderDate = o.OrderDate,

                    TotalAmount = o.TotalAmount,

                    FullName = o.FullName,

                    Email = o.Email,

                    Address = o.Address,

                    OrderItems =
                        o.OrderItems.Select(oi =>
                            new OrderItemResponseDTO
                            {
                                ProductId = oi.ProductId,

                                ProductName = oi.ProductName,

              

                                Quantity = oi.Quantity,

                                Price = oi.Price,
                                 SelectedSize = oi.SelectedSize
                            }).ToList()
                }).ToList();
        }
    }
}
