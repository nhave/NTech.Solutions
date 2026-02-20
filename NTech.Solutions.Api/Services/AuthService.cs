using NTech.Solutions.Api.Repositories;
using NTech.Solutions.Common.Helpers;
using NTech.Solutions.Common.Models.Dtos;

namespace NTech.Solutions.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IJwtService jwtService;
        private readonly IWebHostEnvironment environment;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, IWebHostEnvironment environment)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.jwtService = jwtService;
            this.environment = environment;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (dto.Password != dto.PasswordConfirmation)
                throw new ArgumentException("Passwords do not match.");
            if (!PasswordHelper.IsPasswordSecure(dto.Password))
                throw new ArgumentException("Password does not meet security requirements.");

            if (userRepository.GetByEmailAsync(dto.Email).Result != null)
                throw new ArgumentException("Email is already registered.");

            var user = await userRepository.CreateAsync(dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));
            if (user != null)
            {
                return new RegisterResponseDto { UserId = user.Id };
            }
            else
            {
                throw new Exception("Failed to create user.");
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await userRepository.GetByEmailAsync(dto.Email);
            if (user == null || user.PasswordHash == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Invalid email or password.");

            return await jwtService.CreateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshAsync(RefreshTokenDto dto)
        {
            var user = await refreshTokenRepository.ValidateAsync(dto.RefreshToken);
            if (user == null)
                throw new ArgumentException("Refresh token is not valid.");

            return await jwtService.CreateAuthResponseAsync(user);
        }
    }
}
