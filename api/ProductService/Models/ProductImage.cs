using System.ComponentModel.DataAnnotations;

namespace Productservice.Models
{
    public class ProductImage
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; }
            = string.Empty;
    }
}
