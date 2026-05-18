using System.ComponentModel.DataAnnotations;

namespace Productservice.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
            = string.Empty;
    }
}
