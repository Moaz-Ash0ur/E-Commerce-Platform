using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApi.Services;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using System.Security.Claims;

namespace OnlineShoppingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of all orders.</returns>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = _orderService.GetAll();

            if (orders == null)
                return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orders, "No Orders to retrieved."));


            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orders, "Orders retrieved successfully."));
        }

        /// <summary>
        /// Get all orders with items details.
        /// </summary>
        /// <returns>List of all orders.</returns>
        [HttpGet("GetAllDetails")]
        public ActionResult<ApiResponse<IEnumerable<OrderDto>>> GetAllDetails()
        {
            var orders = _orderService.GetAllAsQueryable();

            if(orders == null)
                return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orders, "No Orders to retrieved."));


            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orders, "Orders retrieved successfully."));
        }


        /// <summary>
        /// Get order by its Id.
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<OrderDto>> GetById(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound(ApiResponse<OrderDto>.FailResponse("Order not found."));

            return Ok(ApiResponse<OrderDto>.SuccessResponse(order, "Order retrieved successfully."));
        }

        /// <summary>
        /// Create a new order (Checkout).
        /// </summary>
        /// <returns>Checkout completion message</returns>
        [HttpPost("checkout")]
        public ActionResult<ApiResponse<string>> Checkout()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return BadRequest(ApiResponse<string>.FailResponse("Invalid user ID."));

            try
            {
                _orderService.Checkout(userIdClaim);
                return Ok(ApiResponse<string>.SuccessMessage("Checkout completed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message, "Checkout failed."));
            }
        }

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="dto">Order data</param>
        /// <returns>Update result</returns>
        [HttpPut]
        public ActionResult<ApiResponse<string>> Update(OrderDto dto)
        {
            try
            {
                _orderService.Update(dto);
                return Ok(ApiResponse<string>.SuccessMessage("Order updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message, "Failed to update order."));
            }
        }

        /// <summary>
        /// Delete an order by Id.
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<string>> Delete(int id)
        {
            try
            {
                _orderService.Delete(id);
                return Ok(ApiResponse<string>.SuccessMessage("Order deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message, "Failed to delete order."));
            }
        }



    }


}
