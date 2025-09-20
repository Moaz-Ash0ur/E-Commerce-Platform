using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;

namespace ShoppingBLL.Contracts
{
    public interface ICartService
    {
        CartDto GetCartByCustomer(string CustomerId);
        void AddToCart(string customerId , CartItemDto Dto);
        void RemoveFromCart(string customerId , int productId);
        void ClearCart(string customerId );
    }






}
