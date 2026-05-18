using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // From UserService JWT
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }
            = DateTime.UtcNow;

        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        // Navigation
        public ICollection<OrderItem> OrderItems
        { get; set; } = new List<OrderItem>();
    }
}
