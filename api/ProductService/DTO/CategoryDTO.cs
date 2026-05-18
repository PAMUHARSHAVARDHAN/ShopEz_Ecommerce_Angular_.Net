using System.ComponentModel.DataAnnotations;

namespace Productservice.DTO
{
    public class CategoryDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
             = string.Empty;
    }
}
