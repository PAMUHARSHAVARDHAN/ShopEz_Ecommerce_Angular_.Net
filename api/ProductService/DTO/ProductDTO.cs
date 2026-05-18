using System.ComponentModel.DataAnnotations;

namespace Productservice.DTO
{
    public class ProductDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
            = string.Empty;

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public string Description { get; set; }
            = string.Empty;


        [Required]
        [Range(0, 100000)]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(20)]
        public string Gender { get; set; }
          = string.Empty;
        public List<ProductImageDTO> ProductImages { get; set; }
           = new();
        public List<ProductSizeDTO> ProductSizes { get; set; }
    = new();
    }
}
