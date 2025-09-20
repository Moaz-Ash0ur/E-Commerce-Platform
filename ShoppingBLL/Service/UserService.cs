using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs.User;
using ShoppingDAL.Domains;
using System.Security.Claims;

namespace ShoppingBLL.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IConfiguration config,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<UserResultDto> RegisterAsync(RegisterDto dto)       
        {      

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.Phone
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return FailResult(createResult.Errors);

            var roleName = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role;
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
                return FailResult(roleResult.Errors);

            return new UserResultDto { Success = true }; ;
        }
        private UserResultDto FailResult(IEnumerable<IdentityError> errors)
        {
            return new UserResultDto
            {
                Success = false,
                Errors = errors.Select(e => e.Description)
            };
        } 
        public async Task<UserResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new UserResultDto { Success = false, Errors = new[] { "Invalid email or password" } };
         
            return new UserResultDto { Success = true };
        }
        public string GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId!;
        }

        //User
        public async Task<UserDto> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);

        }

        public async Task<UserDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }


        //For Authn
        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Role = roles.FirstOrDefault()!
            };
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Role = roles.FirstOrDefault()!
            };
        }

        public async Task<(Claim[], UserDto)> GetClaims(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return (claims, user);
        }

        public async Task<Claim[]> GetClaimsById(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return claims;
        }



    }


}
