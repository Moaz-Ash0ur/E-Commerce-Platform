using ShoppingBLL.DTOs.User;
using System.Security.Claims;

namespace ShoppingBLL.Contracts
{
    public interface IUserService
    {
        Task<UserResultDto> RegisterAsync(RegisterDto dto);
        Task<UserResultDto> LoginAsync(LoginDto dto);
        string GetLoggedInUser();


        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> GetUserProfileAsync(Guid UserId);

        Task<UserDto> GetUserByIdAsync(Guid UserId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<(Claim[], UserDto)> GetClaims(string email);
        Task<Claim[]> GetClaimsById(Guid UserId);




    }









}
