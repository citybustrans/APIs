using System.ComponentModel.DataAnnotations;
using CBT.API.Models.Entities;

namespace CBT.API.Models.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public DateTime DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string EmailOrPhone { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserDto
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }

    public class GoogleAuthDto
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }
    }

    public class SocialAuthDto
    {
        [Required]
        public string Provider { get; set; } = string.Empty;

        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }
    }
}
