using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderService.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        // ProductService reference
        public int ProductId { get; set; }

        // Snapshot data
        [Required]
        public string ProductName { get; set; }
            = string.Empty;

       
        [Required]
        public string SelectedSize { get; set; }
    = string.Empty;


        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
