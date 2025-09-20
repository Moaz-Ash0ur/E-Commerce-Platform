using ECommerceStore.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApi.Services;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;

namespace OnlineShoppingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = _categoryService.GetAll();
            return Ok(ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categories, "Categories retrieved successfully."));
        }

        /// <summary>
        /// Get category by Id.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CategoryDto>> GetById(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound(ApiResponse<CategoryDto>.FailResponse($"Category with id={id} not found.", "Category not found."));

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(category, "Category retrieved successfully."));
        }

        /// <summary>
        /// Create new category.
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<CategoryDto>> Create(CategoryDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<CategoryDto>.FailResponse("Invalid category data."));

            if (_categoryService.Add(dto, out int id))
            {
                dto.Id = id;
                return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<CategoryDto>.SuccessResponse(dto, "Category created successfully."));
            }

            return BadRequest(ApiResponse<CategoryDto>.FailResponse("Failed to Add category."));
        }

        /// <summary>
        /// Update existing category.
        /// </summary>
        [HttpPut("{categoryId}")]
        public ActionResult<ApiResponse<CategoryDto>> Update(int categoryId , CategoryDto dto)
        {
            var existing = _categoryService.GetById(categoryId);
            if (existing == null)
                return NotFound(ApiResponse<CategoryDto>.FailResponse($"Category with id={categoryId} not found.", "Category not found."));

            dto.Id = categoryId;
            var updated = _categoryService.Update(dto);
            if (!updated)
                return BadRequest(ApiResponse<CategoryDto>.FailResponse("Failed to update category.", "Update error."));

            return Ok(ApiResponse<CategoryDto>.SuccessResponse(dto, "Category updated successfully."));
        }

        /// <summary>
        /// Delete category by Id.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object>> Delete(int id)
        {
            var existing = _categoryService.GetById(id);
            if (existing == null)
                return NotFound(ApiResponse<object>.FailResponse($"Category with id={id} not found.", "Category not found."));

            var deleted = _categoryService.Delete(id);
            if (!deleted)
                return BadRequest(ApiResponse<object>.FailResponse("Failed to delete category.", "Delete error."));

            return Ok(ApiResponse<object>.SuccessMessage("Category deleted successfully."));
        }
    }


}
