using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using System.Security.Claims;

namespace OnlineShoppingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public IActionResult AddToCart([FromBody] CartItemDto cartItem)
        {
            if (cartItem == null || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
                return BadRequest("Invalid cart item data.");

            var userId = GetUserIdByClaims();
            if (userId == null)
                return BadRequest("Invalid user ID.");

            _cartService.AddToCart(userId, cartItem);
            return Ok("Product added to cart successfully.");
        }


        [HttpGet("get-cart")]
        public IActionResult GetCart()
        {
            var userId = GetUserIdByClaims();
            if (userId == null)
                return BadRequest("Invalid user ID.");

            var cart = _cartService.GetCartByCustomer((userId));
            return Ok(cart);
        }


        [HttpDelete("remove-from-cart/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            if (productId <= 0)
                return BadRequest("Invalid product ID.");

            var userId = GetUserIdByClaims();
            if (userId == null)
                return BadRequest("Invalid user ID.");

            _cartService.RemoveFromCart(userId, productId);
            return Ok("Product removed from cart successfully.");
        }


        private string? GetUserIdByClaims()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }



    }
}
