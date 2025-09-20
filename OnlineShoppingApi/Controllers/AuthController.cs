using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApi.Services;
using OnlineShoppingApi.Services.AuthnService;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs.User;

namespace OnlineShoppingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

       // private readonly IUserService _userService;
         private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
           // _userService = userService;
            _authService = authService;
        }


        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">User registration details (email, password, fullname...)</param>
        /// <returns>ApiResponse with user details</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result.Success == false)
                return BadRequest(ApiResponse<AuthResult>.FailResponse($"Failed to register user {result.Errors.FirstOrDefault()}"));

            return Ok(ApiResponse<string>.SuccessMessage("User registered successfully"));
        }

        /// <summary>
        /// Login with email and password to get access & refresh tokens.
        /// </summary>
        /// <param name="request">Login DTO (email + password)</param>
        /// <returns>ApiResponse with JWT tokens</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(ApiResponse<string>.FailResponse(result.Errors.ToList(), "Invalid login attempt"));

            SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);

            return Ok(ApiResponse<string>.SuccessResponse(result.AccessToken, "Login successful"));
        }

        /// <summary>
        /// Refresh access token using a valid refresh token.
        /// </summary>
        /// <returns>ApiResponse with new JWT tokens</returns>
        [HttpPost("refreshAccessToken")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> RefreshAccessToken()
        {
            var refreshToken = GetRefreshTokenCookie();

            var result = await _authService.RefreshAccessTokenAsync(refreshToken);

            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResult>.FailResponse(result.Errors.ToList(), "Failed to refresh token"));


            return Ok(ApiResponse<AuthResult>.SuccessResponse(result, "Token refreshed successfully"));
        }



        //Set and Get Token in Client-Browser Http only Cookie
        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookies = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookies);
        }
        private string GetRefreshTokenCookie()
        {
            return Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) ? refreshToken : string.Empty;
        }


      
    }


}
