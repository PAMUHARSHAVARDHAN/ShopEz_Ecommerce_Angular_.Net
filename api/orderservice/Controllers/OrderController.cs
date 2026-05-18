using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Services;
using System.Security.Claims;

namespace orderservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // Create Order
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult>
            Create(CreateOrderDTO dto)
        {
            try
            {
                var userIdClaim = User.Claims
                    .FirstOrDefault(c =>
                        c.Type ==
                        ClaimTypes.NameIdentifier)
                    ?.Value;

                if (userIdClaim == null)
                    return Unauthorized();

                int userId = int.Parse(userIdClaim);
                dto.UserId = userId;

                var order =
                    await _service.CreateOrderAsync(dto, userId);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = order.OrderId },
                    order);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.InnerException?.Message
                    ?? ex.Message);
            }
        }

        // Get All Orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _service.GetAllOrdersAsync());
        }

        // Get Order By Id
        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetById(int id)
        {
            var order =
                await _service.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // Current User Orders
        [HttpGet("my-orders")]
        public async Task<IActionResult>
            GetMyOrders()
        {
            var userIdClaim = User.Claims
                .FirstOrDefault(c =>
                    c.Type ==
                    ClaimTypes.NameIdentifier)
                ?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var orders =
                await _service
                .GetOrdersByUserIdAsync(userId);

            return Ok(orders);
        }
    }
}
