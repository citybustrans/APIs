using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CBT.API.Models.DTOs;
using CBT.API.Models.Entities;
using CBT.API.Services.Interfaces;

namespace CBT.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool Success, string Message, UserResponseDto? User, string? Token)> RegisterAsync(RegisterUserDto registerDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return (false, "User with this email already exists", null, null);
                }

                if (!string.IsNullOrEmpty(registerDto.PhoneNumber))
                {
                    var existingPhoneUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == registerDto.PhoneNumber);
                    if (existingPhoneUser != null)
                    {
                        return (false, "User with this phone number already exists", null, null);
                    }
                }

                var user = new User
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Role = registerDto.Role,
                    DateOfBirth = registerDto.DateOfBirth,
                    Address = registerDto.Address,
                    EmailConfirmed = true // For demo purposes
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Registration failed: {errors}", null, null);
                }

                var token = await GenerateJwtTokenAsync(user);
                var userResponse = MapToUserResponseDto(user);

                return (true, "Registration successful", userResponse, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return (false, "An error occurred during registration", null, null);
            }
        }

        public async Task<(bool Success, string Message, UserResponseDto? User, string? Token)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                User? user = null;

                // Try to find user by email first
                if (loginDto.EmailOrPhone.Contains("@"))
                {
                    user = await _userManager.FindByEmailAsync(loginDto.EmailOrPhone);
                }
                else
                {
                    // Try to find by phone number
                    user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == loginDto.EmailOrPhone);
                }

                if (user == null)
                {
                    return (false, "Invalid credentials", null, null);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return (false, "Invalid credentials", null, null);
                }

                if (!user.IsActive)
                {
                    return (false, "Account is deactivated", null, null);
                }

                var token = await GenerateJwtTokenAsync(user);
                var userResponse = MapToUserResponseDto(user);

                return (true, "Login successful", userResponse, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return (false, "An error occurred during login", null, null);
            }
        }

        public async Task<(bool Success, string Message, UserResponseDto? User, string? Token)> GoogleAuthAsync(GoogleAuthDto googleAuthDto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleAuthDto.IdToken);

                var user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    // Create new user
                    user = new User
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FirstName = payload.GivenName ?? "",
                        LastName = payload.FamilyName ?? "",
                        Role = googleAuthDto.Role,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return (false, $"Google authentication failed: {errors}", null, null);
                    }
                }

                if (!user.IsActive)
                {
                    return (false, "Account is deactivated", null, null);
                }

                var token = await GenerateJwtTokenAsync(user);
                var userResponse = MapToUserResponseDto(user);

                return (true, "Google authentication successful", userResponse, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication");
                return (false, "Google authentication failed", null, null);
            }
        }

        public async Task<(bool Success, string Message, UserResponseDto? User, string? Token)> SocialAuthAsync(SocialAuthDto socialAuthDto)
        {
            try
            {
                // This is a simplified implementation
                // In a real application, you would validate the access token with the respective social media provider

                // For demo purposes, we'll create a mock validation
                var socialUserInfo = await ValidateSocialToken(socialAuthDto.Provider, socialAuthDto.AccessToken);
                if (socialUserInfo == null)
                {
                    return (false, "Invalid social media token", null, null);
                }

                var user = await _userManager.FindByEmailAsync(socialUserInfo.Email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = socialUserInfo.Email,
                        Email = socialUserInfo.Email,
                        FirstName = socialUserInfo.FirstName,
                        LastName = socialUserInfo.LastName,
                        Role = socialAuthDto.Role,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return (false, $"Social authentication failed: {errors}", null, null);
                    }
                }

                if (!user.IsActive)
                {
                    return (false, "Account is deactivated", null, null);
                }

                var token = await GenerateJwtTokenAsync(user);
                var userResponse = MapToUserResponseDto(user);

                return (true, "Social authentication successful", userResponse, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during social authentication");
                return (false, "Social authentication failed", null, null);
            }
        }

        public async Task<(bool Success, string Message)> LogoutAsync(string userId)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return (true, "Logout successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return (false, "An error occurred during logout");
            }
        }

        public async Task<(bool Success, string Message)> RefreshTokenAsync(string refreshToken)
        {
            // Implementation for refresh token logic
            // This would typically involve validating the refresh token and generating a new access token
            await Task.CompletedTask;
            return (false, "Refresh token functionality not implemented");
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? ""),
                new(ClaimTypes.Email, user.Email ?? ""),
                new("role", user.Role.ToString()),
                new("firstName", user.FirstName),
                new("lastName", user.LastName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["ExpirationInDays"] ?? "7")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "");

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

               tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        private async Task<SocialUserInfo?> ValidateSocialToken(string provider, string accessToken)
        {
            // This is a mock implementation
            // In a real application, you would make HTTP requests to the respective social media APIs
            await Task.Delay(100); // Simulate API call

            return new SocialUserInfo
            {
                Email = "user@example.com",
                FirstName = "Social",
                LastName = "User"
            };
        }

        private class SocialUserInfo
        {
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }
    }
}
