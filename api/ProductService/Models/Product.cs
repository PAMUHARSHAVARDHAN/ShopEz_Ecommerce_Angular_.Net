using Productservice.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productservice.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
            = string.Empty;

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public string? Description { get; set; }

     

        [Range(0, 100000)]
        public int Stock { get; set; }

        public bool IsActive { get; set; }
            = true;

        public int CategoryId { get; set; }
        public string Gender { get; set; }

        // Snapshot Category Name
        public string? CategoryName { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    = new();
        public List<ProductSize> ProductSizes { get; set; }
            = new();
    }
}
