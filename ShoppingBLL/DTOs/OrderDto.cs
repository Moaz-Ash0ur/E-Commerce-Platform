namespace ShoppingBLL.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }        
        public List<OrderItemDto> Items { get; set; }
    }
}
