using CBT.API.Models.DTOs;
using CBT.API.Models.Entities;

namespace CBT.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, UserResponseDto? User, string? Token)> RegisterAsync(RegisterUserDto registerDto);
        Task<(bool Success, string Message, UserResponseDto? User, string? Token)> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string Message, UserResponseDto? User, string? Token)> GoogleAuthAsync(GoogleAuthDto googleAuthDto);
        Task<(bool Success, string Message, UserResponseDto? User, string? Token)> SocialAuthAsync(SocialAuthDto socialAuthDto);
        Task<(bool Success, string Message)> LogoutAsync(string userId);
        Task<(bool Success, string Message)> RefreshTokenAsync(string refreshToken);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<bool> ValidateTokenAsync(string token);
    }
}
