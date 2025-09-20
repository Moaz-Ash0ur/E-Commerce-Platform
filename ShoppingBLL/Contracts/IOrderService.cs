using ShoppingBLL.DTOs;

namespace ShoppingBLL.Contracts
{

    public interface IOrderService
    {
        public IEnumerable<OrderDto> GetAll();
        public OrderDto? GetById(int id);
        public void Checkout(string customerId);
        public bool Update(OrderDto order);
        public bool Delete(int id);
        public List<OrderDto> GetAllAsQueryable();
    }



}
