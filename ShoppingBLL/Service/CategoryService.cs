using AutoMapper;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;

namespace ShoppingBLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            var categories = _categoryRepo.GetAll();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public CategoryDto? GetById(int id)
        {
            var category = _categoryRepo.GetById(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public bool Add(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return false;

            var category = _mapper.Map<Category>(categoryDto);
            return _categoryRepo.Add(category);
        }

        public bool Add(CategoryDto categoryDto,out int Id)
        {
            Id = 0;

            if (categoryDto == null)
                return false;

            var category = _mapper.Map<Category>(categoryDto);
            if (_categoryRepo.Add(category))
            {
                Id = category.Id;
                return true;
            }
            return false;
        }

        public bool Update(CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return false;

            var existing = _categoryRepo.GetById(categoryDto.Id);
            if (existing == null)
                return false;

            _mapper.Map(categoryDto, existing);

            return _categoryRepo.Update(existing);
        }

        public bool Delete(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return false;

            return _categoryRepo.Delete(category);
        }

    }

}
