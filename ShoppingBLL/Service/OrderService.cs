using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;

namespace ShoppingBLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Order> orderRepo, IMapper mapper, ICartService cartService)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _cartService = cartService;
        }

        public IEnumerable<OrderDto> GetAll()
        {
            var orders = _orderRepo.GetAll();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public List<OrderDto> GetAllAsQueryable()
        {
            var query = _orderRepo.GetAllAsQueryable()
                                  .Include(o => o.Items);
                                  
            var orders= query.ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public OrderDto? GetById(int id)
        {
            var order = _orderRepo.GetById(id);
            return _mapper.Map<OrderDto>(order);
        }


        #region Checkout (Convert Cart to Order)

        public void Checkout(string customerId)
        {
            var cart = GetCustomerCart(customerId);

            var order = InitializeOrder(customerId);

            AddOrderItems(order, cart);

            ClearCustomerCart(customerId);
        }

        private CartDto GetCustomerCart(string customerId)
        {
            var cart = _cartService.GetCartByCustomer(customerId);
            if (cart == null || !cart.Items.Any())
                return null!;

            return cart;
        }

        private Order InitializeOrder(string customerId)
        {
            return new Order
            {
                CustomerId = customerId,              
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                Items = new List<OrderItem>()
            };
        }

        private void AddOrderItems(Order order, CartDto cart)
        {
            foreach (var item in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });

                order.TotalAmount += item.UnitPrice * item.Quantity;
            }

            _orderRepo.Add(order);
        }

        private void ClearCustomerCart(string customerId)
        {
            _cartService.ClearCart(customerId);
        }


        #endregion
        public bool Update(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            if (_orderRepo.Update(order))
            {
                _orderRepo.Save();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order != null)
            {
                if (_orderRepo.Delete(order))
                {
                    _orderRepo.Save();
                     return  true;
                }

            }
            return false;

        }

    }


}
