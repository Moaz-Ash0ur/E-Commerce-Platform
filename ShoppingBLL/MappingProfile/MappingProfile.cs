using AutoMapper;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingBLL.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Order, OrderDto>()
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
             .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();


            CreateMap<Cart, CartDto>()
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems))
             .ReverseMap();

            CreateMap<CartItem, CartItemDto>().ReverseMap();


            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();

        }


    }







}
