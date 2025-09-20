using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;

namespace ShoppingBLL.Services
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepo;
        private readonly IGenericRepository<CartItem> _cartItemRepo;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CartService(IGenericRepository<Cart> cartRepo, IGenericRepository<CartItem> cartItemRepo
       ,IMapper mapper,IProductService productService)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
            _productService = productService;
        }

        public CartDto GetCartByCustomer(string customerId)
        {
            var cart = _cartRepo.GetAllAsQueryable()
                .Include(c => c.CartItems)
                .Where(c => c.CustomerId.ToString() == customerId)
                .FirstOrDefault();

            return _mapper.Map<CartDto>(cart);
        }

        private Cart GetCartEntity(string customerId)
        {
            return _cartRepo.GetAllAsQueryable()
                .Include(c => c.CartItems)             
                .FirstOrDefault(c => c.CustomerId.ToString() == customerId)!;
        }

        // Cart Proccess
        public void AddToCart(string customerId, CartItemDto Dto)
        {
            var cart = GetCartEntity(customerId);

            if (cart == null)
            {
                 cart = new Cart { CustomerId = customerId };
                _cartRepo.Add(cart);
            }

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == Dto.ProductId);
            if (item != null)
            {
                item.Quantity += Dto.Quantity;
                _cartItemRepo.Update(item);
            }
            else
            {
                _cartItemRepo.Add(new CartItem { CartId = cart.Id, ProductId = Dto.ProductId, 
                    Quantity = Dto.Quantity , UnitPrice = Dto!.UnitPrice});
            }

        }


        public void RemoveFromCart(string customerId, int productId)
        {
            var cart = GetCartEntity(customerId);

            if (cart == null) return;

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (item != null)
            {
                _cartItemRepo.Delete(item);
            }
        }


        public void ClearCart(string customerId)
        {

            var cart = GetCartEntity(customerId);

            if (cart != null)
            {
                foreach (var item in cart.CartItems.ToList())
                {
                    _cartItemRepo.Delete(item);
                }
            }


        }



        ////change qty and delete
        //public void RemoveFromCart(string customerId, int productId)
        //{
        //    var cart = GetCartEntity(customerId);

        //    if (cart == null) return;

        //    var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);


        //    if (item!.Quantity > 1)
        //    {
        //        item.Quantity--;
        //        _cartItemRepo.Update(item);
        //    }
        //    else
        //    {
        //        _cartItemRepo.Delete(item);
        //    }

        //}

    }
}
