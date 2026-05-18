namespace Productservice.Models
{
    public class ProductSize
    {
        public int ProductSizeId { get; set; }

        public int ProductId { get; set; }

        public string SizeValue { get; set; }
            = string.Empty;

        public int Stock { get; set; }
    }
}
