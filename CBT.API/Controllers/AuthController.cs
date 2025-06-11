using CBT.API.Models.DTOs;
using CBT.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CBT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">User registration details</param>
        /// <returns>User details and authentication token</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                user = result.User,
                token = result.Token
            });
        }

        /// <summary>
        /// Login with email/phone and password
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>User details and authentication token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                user = result.User,
                token = result.Token
            });
        }

        /// <summary>
        /// Authenticate with Google
        /// </summary>
        /// <param name="googleAuthDto">Google authentication details</param>
        /// <returns>User details and authentication token</returns>
        [HttpPost("google")]
        public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthDto googleAuthDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.GoogleAuthAsync(googleAuthDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                user = result.User,
                token = result.Token
            });
        }

        /// <summary>
        /// Authenticate with social media (Facebook, Twitter, Instagram)
        /// </summary>
        /// <param name="socialAuthDto">Social media authentication details</param>
        /// <returns>User details and authentication token</returns>
        [HttpPost("social")]
        public async Task<IActionResult> SocialAuth([FromBody] SocialAuthDto socialAuthDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.SocialAuthAsync(socialAuthDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                user = result.User,
                token = result.Token
            });
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns>Logout confirmation</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Invalid user" });
            }

            var result = await _authService.LogoutAsync(userId);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        /// <summary>
        /// Refresh authentication token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>New authentication token</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        /// <summary>
        /// Validate authentication token
        /// </summary>
        /// <param name="token">Token to validate</param>
        /// <returns>Token validation result</returns>
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var isValid = await _authService.ValidateTokenAsync(token);

            return Ok(new { isValid });
        }
    }
}
