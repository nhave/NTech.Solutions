using System.ComponentModel.DataAnnotations;

namespace NTech.Solutions.Common.Models.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(64, ErrorMessage = "Password must contain atleast 8 characters.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_])[^\s]{8,}$", ErrorMessage = "Password is not secure.")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; } = null!;
    }

    public class RegisterResponseDto
    {
        public string UserId { get; set; } = null!;
    }

    public class InternalRegisterDto : RegisterResponseDto
    {
        public string ValidationToken { get; set; } = null!;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public int Expires { get; set; }
    }
}
