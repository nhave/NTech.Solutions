using NTech.Solutions.Common.Models.Database;
using NTech.Solutions.Common.Models.Dtos;

namespace NTech.Solutions.Api.Services
{
    /// <summary>
    /// Defines methods for generating, validating, and managing JSON Web Tokens (JWT) and refresh tokens for user
    /// authentication.
    /// </summary>
    /// <remarks>Implementations of this interface provide functionality for issuing JWTs, creating refresh
    /// tokens, validating refresh tokens, and assembling authentication responses. This interface is intended for use
    /// in authentication workflows where secure token-based user identification is required.</remarks>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user.
        /// </summary>
        /// <remarks>The returned token can be used for authentication and authorization in client
        /// applications. Ensure that the user object contains all required claims or properties for token
        /// generation.</remarks>
        /// <param name="user">The user for whom the JWT will be generated. Cannot be null.</param>
        /// <returns>A string containing the generated JWT for the user.</returns>
        public string GenerateJwtToken(User user);

        /// <summary>
        /// Generates a new refresh token asynchronously for use in authentication workflows.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the newly
        /// generated refresh token.</returns>
        public Task<string> GenerateRefreshTokenAsync();

        /// <summary>
        /// Creates an authentication response for the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user for whom the authentication response is generated. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an authentication response for
        /// the specified user.</returns>
        public Task<AuthResponseDto> CreateAuthResponseAsync(User user);
    }
}
