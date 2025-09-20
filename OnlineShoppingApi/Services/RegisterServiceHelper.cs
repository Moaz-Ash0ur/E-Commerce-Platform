using ECommerceStore.BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApi.Services.AuthnService;
using ShoppingBLL.Contracts;
using ShoppingBLL.MappingProfile;
using ShoppingBLL.Services;
using ShoppingDAL.Data;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;

namespace OnlineShoppingApi.Services
{
    public static class RegisterServiceHelper
    {

        public static void RegisterService(this WebApplicationBuilder builder)
        {


            builder.Services.AddSwaggerGenJwtAuth();

            //Configration Shopping Project
            builder.Services.AddDbContext<ShoppingDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ShoppingDbContext>()
            .AddDefaultTokenProviders();

            // JWT
            builder.Services.CustomJwtAuthConfig(builder.Configuration);

        }

    }

}
