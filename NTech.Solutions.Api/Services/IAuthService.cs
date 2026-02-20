using NTech.Solutions.Common.Models.Dtos;

namespace NTech.Solutions.Api.Services
{
    /// <summary>
    /// Defines methods for user authentication and registration operations.
    /// </summary>
    /// <remarks>Implementations of this interface provide functionality for registering new users and
    /// managing authentication workflows. Methods are typically asynchronous to support non-blocking operations, such
    /// as database access or external service calls.</remarks>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user asynchronously using the provided registration details.
        /// </summary>
        /// <param name="dto">The registration information for the new user. Cannot be null and must contain valid user data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a RegisterResponseDto with the
        /// outcome of the registration process.</returns>
        public Task<RegisterResponseDto> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Authenticates a user asynchronously using the provided login credentials.
        /// </summary>
        /// <param name="dto">An object containing the user's login credentials. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthResponseDto with
        /// authentication details if the login is successful.</returns>
        public Task<AuthResponseDto> LoginAsync(LoginDto dto);

        /// <summary>
        /// Asynchronously refreshes the authentication tokens using the specified refresh token information.
        /// </summary>
        /// <param name="dto">An object containing the refresh token and related data required to obtain new authentication tokens. Cannot
        /// be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="AuthResponseDto"/>
        /// with the new authentication tokens if the refresh is successful.</returns>
        public Task<AuthResponseDto> RefreshAsync(RefreshTokenDto dto);
    }
}
