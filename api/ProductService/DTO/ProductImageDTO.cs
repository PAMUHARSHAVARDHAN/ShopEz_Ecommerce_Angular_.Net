using System.ComponentModel.DataAnnotations;

namespace Productservice.DTO
{
    public class ProductImageDTO
    {
        [Required]
        public string ImageUrl { get; set; }
            = string.Empty;
    }
}
