using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApi.Services;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;

namespace OnlineShoppingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products.</returns>
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<ProductDto>>> GetAll()
        {
            var products = _productService.GetAll();
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(products, "Products retrieved successfully."));
        }


        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products.</returns>
        [HttpGet("GetMostOrderedProducts")]
        public ActionResult<ApiResponse<IEnumerable<ProductDto>>> GetMostOrderedProducts()
        {
            var products = _productService.GetMostOrderedProducts();
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(products, "Most Ordered Products retrieved successfully."));
        }


        /// <summary>
        /// Get all products (with category info).
        /// </summary>
        /// <returns>List of products depand Category ID.</returns>
        [HttpGet("GetAllByCategory/{categoryId}")]
        public ActionResult<ApiResponse<IEnumerable<ProductDto>>> GetAllByCategory(int categoryId)
        {
            var products = _productService.GetAllByCategory(categoryId);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(products, "Products retrieved successfully."));
        }

        /// <summary>
        /// Get product by Id.
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Product object if found.</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ProductDto>> GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(ApiResponse<ProductDto>.FailResponse($"Product with id={id} not found.", "Product not found."));

            return Ok(ApiResponse<ProductDto>.SuccessResponse(product, "Product retrieved successfully."));
        }


        /// <summary>
        /// Create new product.
        /// </summary>
        /// <param name="dto">Product data</param>
        /// <returns>Result of creation.</returns>
        [HttpPost("Add")]
        public IActionResult Add([FromForm] ProductDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<ProductDto>.FailResponse("Invalid product data."));

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imageName = UploadFile.UploadImage(dto.Image);
                dto.ImageUrl = imageName;
            }

            if (_productService.Add(dto, out int id))
            {
                dto.Id = id;

                if (!string.IsNullOrEmpty(dto.ImageUrl))
                    dto.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/Products/{dto.ImageUrl}";

                return CreatedAtAction(nameof(GetById), new { id },
                    ApiResponse<ProductDto>.SuccessResponse(dto, "Product created successfully."));
            }

            return BadRequest(ApiResponse<ProductDto>.FailResponse("Failed to add product."));
        }


        /// <summary>
        /// Update existing product.
        /// </summary>
        /// <param name="dto">Updated product data</param>
        /// <returns>Result of update.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] ProductDto dto)
        {
            var existingProduct = _productService.GetById(id);
            if (existingProduct == null)
                return NotFound(ApiResponse<string>.FailResponse("Product not found."));

            if (dto.Image != null && dto.Image.Length > 0)
            {
                dto.ImageUrl = UploadFile.UploadImage(dto.Image, existingProduct.ImageUrl);
            }
            else
            {
                dto.ImageUrl = existingProduct.ImageUrl;
            }
            dto.Id = id;
            if (_productService.Update(dto))
                return Ok(ApiResponse<ProductDto>.SuccessResponse(dto, "Product updated successfully."));

            return BadRequest(ApiResponse<string>.FailResponse("Failed to update product."));
        }

                                                                   

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Result of delete.</returns>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object>> Delete(int id)
        {
            var existing = _productService.GetById(id);
            if (existing == null)
                return NotFound(ApiResponse<object>.FailResponse($"Product with id={id} not found.", "Product not found."));

            var result = _productService.Delete(id);
            return result
                ? Ok(ApiResponse<object>.SuccessMessage("Product deleted successfully."))
                : BadRequest(ApiResponse<object>.FailResponse("Failed to delete product."));
        }


        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(int id , IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<object>.FailResponse("No file uploaded."));

            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(ApiResponse<object>.FailResponse($"Product with id={id} not found."));

            var imageUrl = UploadFile.UploadImage(file);
            product.ImageUrl = imageUrl;

            if (_productService.Update(product))
            {
                return Ok(ApiResponse<ProductDto>.SuccessResponse(product, "Image uploaded successfully."));
            }

            return BadRequest(ApiResponse<object>.FailResponse("Failed to upload image."));
        }


        [HttpPost("filter")]
        public IActionResult FilterProducts([FromBody] ProductFilterDto filter)
        {
            var result = _productService.ProductFilter(filter);
            return Ok(result);
        }






    }



}
