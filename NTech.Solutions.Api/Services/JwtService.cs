using Microsoft.IdentityModel.Tokens;
using NTech.Solutions.Api.Repositories;
using NTech.Solutions.Common.Models.Database;
using NTech.Solutions.Common.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NTech.Solutions.Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly IRefreshTokenRepository tokenRepository;
        private readonly IConfiguration configuration;

        public JwtService(IRefreshTokenRepository tokenRepository, IConfiguration configuration)
        {
            this.tokenRepository = tokenRepository;
            this.configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var Jwt = configuration.GetSection("Jwt");
            var Issuer = Jwt["Issuer"];
            var Audience = Jwt["Audience"];
            var Secret = Jwt["Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                //new Claim("FirstName", user.FirstName),
                //new Claim("LastName", user.LastName),
                //new Claim("Birthday", user.Birthday.ToString()),
                new Claim("email", user.Email)
            };

            //foreach (var role in user.Roles)
            //{
            //    Claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            //}

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            while (true)
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                    var token = Convert.ToBase64String(randomNumber);
                    if (await tokenRepository.GetAsync(token) == null) return token;
                }
            }
        }

        public async Task<AuthResponseDto> CreateAuthResponseAsync(User user)
        {
            var jwtToken = GenerateJwtToken(user);
            var refreshTokenString = await GenerateRefreshTokenAsync();
            int expiry = 1800;

            // Store the refresh token in the database
            await tokenRepository.CreateAsync(user.Id, refreshTokenString, 30);

            // Return authentication response
            return new AuthResponseDto
            {
                JwtToken = jwtToken,
                RefreshToken = refreshTokenString,
                Expires = expiry
            };
        }
    }
}
