using ShoppingBLL.DTOs.User;
using System.Security.Claims;

namespace OnlineShoppingApi.Services.AuthnService
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto request);
        Task<AuthResult> LoginAsync(LoginDto request);
        Task<AuthResult> RefreshAccessTokenAsync(string refreshToken);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }

}
