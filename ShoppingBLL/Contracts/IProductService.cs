using ShoppingBLL.DTOs;

namespace ShoppingBLL.Contracts
{

    public interface IProductService
    {
        public IEnumerable<ProductDto> GetAll();
        public IEnumerable<ProductDto> GetAllByCategory(int catgoryId);
        public ProductDto? GetById(int id);
        public bool Add(ProductDto product);
        public bool Add(ProductDto productDto, out int Id);
        public bool Update(ProductDto product);
        public bool Delete(int id);
        public List<ProductDto> ProductFilter(ProductFilterDto productFilterDto);
        public List<ProductDto> GetMostOrderedProducts(int top = 5);


    }




}
