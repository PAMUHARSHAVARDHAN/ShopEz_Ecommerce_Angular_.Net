namespace OrderService.DTOs
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string FullName { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string Address { get; set; }
            = string.Empty;

        public List<OrderItemResponseDTO> OrderItems
        { get; set; } = new();
    }
    public class OrderItemResponseDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
            = string.Empty;

       
        public string SelectedSize { get; set; }
           = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
