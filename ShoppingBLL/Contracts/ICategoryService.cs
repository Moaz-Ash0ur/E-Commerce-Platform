using ShoppingBLL.DTOs;

namespace ShoppingBLL.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto? GetById(int id);
        bool Add(CategoryDto category);
        bool Add(CategoryDto categoryDto, out int Id);
        bool Update(CategoryDto category);
        bool Delete(int id);
    }

}
