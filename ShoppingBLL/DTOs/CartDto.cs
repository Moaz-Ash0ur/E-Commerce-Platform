using ShoppingDAL.Domains;

namespace ShoppingBLL.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public decimal TotalPrice => Items.Sum(x => x.Quantity * x.UnitPrice);

    }

}
