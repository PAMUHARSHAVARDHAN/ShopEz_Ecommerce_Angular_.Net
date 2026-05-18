using System.ComponentModel.DataAnnotations;

namespace OrderService.DTOs
{
    public class CreateOrderDTO

    {
        public int UserId { get; set; }
        [Required]
        public string FullName { get; set; }
          = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; }
            = string.Empty;

        [Required]
        public string Address { get; set; }
            = string.Empty;
        [Required]
        public decimal TotalAmount { get; set; }

        [MinLength(1)]
        public List<CreateOrderItemDTO> Items
        { get; set; } = new();
    }
    public class CreateOrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
                 = string.Empty;

    
        public string SelectedSize { get; set; }
          = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

}

