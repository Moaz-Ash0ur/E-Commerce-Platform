using Humanizer;
using Microsoft.IdentityModel.Tokens;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingBLL.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShoppingApi.Services.AuthnService
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokens _refreshTokenService;
        private readonly IConfiguration _config;

        public AuthService(IUserService userService, IRefreshTokens refreshTokenService, IConfiguration config)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _config = config;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto request)
        {

            var existingUser = await _userService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResult
                { 
                    Success = false,
                    Errors = new[] { "Email is already taken" }
                };
            }
                
            var userResult = await _userService.RegisterAsync(request);
            if (!userResult.Success)
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = new[] { userResult.Errors.First() }
                };
            }
            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> LoginAsync(LoginDto request)
        {
            var userResult = await _userService.LoginAsync(request);
            if (!userResult.Success)
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = new[] { "Invalid credentials" }
                };
            }

            var (claims, user) = await _userService.GetClaims(request.Email);

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddDays(7);

            var newRefreshDto = new RefreshTokenDto
            {
                Token = refreshToken,
                UserId = Guid.Parse(user.Id),
                ExpiresAt = expiresAt,
                CurrentState = 0
            };

            _refreshTokenService.Refresh(newRefreshDto);

            return new AuthResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResult> RefreshAccessTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new AuthResult { Success = false, Errors = new[] { "No refresh token found" } };
            }

            if (_refreshTokenService.IsExpireToken(refreshToken))
            {
                return new AuthResult { Success = false, Errors = new[] { "Invalid or expired refresh token" } };
            }

            var refreshData = _refreshTokenService.GetByToken(refreshToken);
            if (refreshData == null)
            {
                return new AuthResult { Success = false, Errors = new[] { "Refresh token not found" } };
            }

            var claims = await _userService.GetClaimsById(refreshData.UserId);
            var newAccessToken = GenerateAccessToken(claims);

            return new AuthResult
            {
                Success = true,
                AccessToken = newAccessToken
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // access short-lived
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }

}
