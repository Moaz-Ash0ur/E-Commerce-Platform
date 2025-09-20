using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;


namespace ECommerceStore.BLL.Services
{
    public  class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<OrderItem> _orderItemRepo;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepo, IMapper mapper, IGenericRepository<OrderItem> orderItemRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _orderItemRepo = orderItemRepo;
        }

        public IEnumerable<ProductDto> GetAll()
        {
            var products = _productRepo.GetAll();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto? GetById(int id)
        {
            var product = _productRepo.GetById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public bool Add(ProductDto productDto)
        {
            if (productDto == null)
                return false;

            var product = _mapper.Map<Product>(productDto);
            return _productRepo.Add(product);
        }

        public bool Add(ProductDto productDto, out int id)
        {
            id = 0;

            if (productDto == null)
                return false;

            var product = _mapper.Map<Product>(productDto);
            if (_productRepo.Add(product))
            {
                id = product.Id;
                return true;
            }

            return false;
        }

        public bool Update(ProductDto productDto)
        {
            if (productDto == null)
                return false;

            var existing = _productRepo.GetById(productDto.Id);
            if (existing == null)
                return false;

            _mapper.Map(productDto, existing);

            return _productRepo.Update(existing);
        }

        public bool Delete(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
                return false;

            return _productRepo.Delete(product); 
        }

        public IEnumerable<ProductDto> GetAllByCategory(int categoryId)
        {
            var products = _productRepo.GetAllAsQueryable().Where(p => p.CategoryId == categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public List<ProductDto> ProductFilter(ProductFilterDto filter)
        {

            var query = _productRepo.GetAllAsQueryable();


            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var name = filter.Name.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(name));
            }

            if (filter.Price.HasValue)
                query = query.Where(p => p.Price >= filter.Price.Value);
        
            if (filter.Rate.HasValue && filter.Rate > 0)
                query = query.Where(p => p.Rate >= filter.Rate.Value);


            var products = query.ToList();

            return _mapper.Map<List<ProductDto>>(products);
        }

        public List<ProductDto> GetMostOrderedProducts(int top = 5)
        {
            var query = _orderItemRepo.GetAllAsQueryable()
                 .Join(
                     _productRepo.GetAllAsQueryable(),
                     oi => oi.ProductId,
                     p => p.Id,
                     (oi, p) => new { oi, p }
                 )
                 .GroupBy(x => new { x.p.Id, x.p.Name, x.p.Price })
                 .Select(g => new ProductDto
                 {
                     Id = g.Key.Id,
                     Name = g.Key.Name,
                     Price = g.Key.Price,
                     TotalOrdered = g.Sum(x => x.oi.Quantity)
                 })
                 .OrderByDescending(dto => dto.TotalOrdered)
                 .Take(top)
                 .ToList();

            return query;
        }


    }


}
