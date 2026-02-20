using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Repositories
{
    /// <summary>
    /// Defines methods for creating, retrieving, validating, revoking, and deleting refresh tokens used for user
    /// authentication.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for managing the lifecycle of refresh
    /// tokens, which are typically used to enable secure, long-lived user sessions. Methods are asynchronous to support
    /// non-blocking operations, such as database or distributed cache access. Thread safety and persistence guarantees
    /// depend on the specific implementation.</remarks>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Asynchronously creates a new refresh token for the specified user with the given token value and expiration
        /// period.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the refresh token is being created. Cannot be null or empty.</param>
        /// <param name="token">The token string to associate with the refresh token. Cannot be null or empty.</param>
        /// <param name="expiryInDays">The number of days until the refresh token expires. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see
        /// cref="RefreshToken"/> instance.</returns>
        public Task<RefreshToken> CreateAsync(string userId, string token, int expiryInDays);

        /// <summary>
        /// Asynchronously retrieves a refresh token that matches the specified token string.
        /// </summary>
        /// <param name="token">The token string to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
        /// cref="RefreshToken"/> if found; otherwise, <see langword="null"/>.</returns>
        public Task<RefreshToken?> GetAsync(string token);

        /// <summary>
        /// Asynchronously validates the specified authentication token and returns the associated user if the token is
        /// valid.
        /// </summary>
        /// <param name="token">The authentication token to validate. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the associated <see
        /// cref="User"/> if the token is valid; otherwise, <see langword="null"/>.</returns>
        public Task<User?> ValidateAsync(string token);

        /// <summary>
        /// Revokes the specified access token asynchronously, invalidating it for future use.
        /// </summary>
        /// <remarks>After revocation, the token can no longer be used to access protected resources. This
        /// method does not throw an exception if the token is already invalid or has been previously revoked.</remarks>
        /// <param name="token">The access token to revoke. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous revoke operation.</returns>
        public Task RevokeAsync(string token);

        /// <summary>
        /// Asynchronously deletes the resource associated with the specified token.
        /// </summary>
        /// <param name="token">The unique identifier of the resource to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public Task DeleteAsync(string token);
    }
}
