using Microsoft.EntityFrameworkCore;
using NTech.Solutions.Api.Data;
using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Repositories
{
    /// <summary>
    /// Provides methods for managing refresh tokens in the application's data store, including adding, retrieving,
    /// revoking, and deleting refresh tokens.
    /// </summary>
    /// <remarks>This repository encapsulates data access operations related to refresh tokens, supporting
    /// asynchronous methods for common token lifecycle management tasks. It is typically used in authentication
    /// workflows to securely handle refresh tokens for user sessions.</remarks>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the RefreshTokenRepostory class using the specified database context.
        /// </summary>
        /// <param name="dbContext">The database context to be used for data access operations. Cannot be null.</param>
        public RefreshTokenRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously creates and persists a new refresh token for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the refresh token is being created. Cannot be null or empty.</param>
        /// <param name="token">The unique token string to assign as the refresh token. Cannot be null or empty.</param>
        /// <param name="expiryInDays">The number of days until the refresh token expires. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see
        /// cref="RefreshToken"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="userId"/> or <paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="expiryInDays"/> is less than or equal to zero.</exception>
        public async Task<RefreshToken> CreateAsync(string userId, string token, int expiryInDays)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            if (expiryInDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(expiryInDays), "Expiry in days must be a positive integer.");

            var refreshToken = new RefreshToken
            {
                Id = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(expiryInDays),
                IsRevoked = false
            };

            await dbContext.AddAsync(refreshToken);
            await dbContext.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// Asynchronously retrieves a refresh token entity that matches the specified token string.
        /// </summary>
        /// <param name="token">The refresh token string to search for. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see
        /// cref="RefreshToken"/> if found; otherwise, <see langword="null"/>.</returns>
        public async Task<RefreshToken?> GetAsync(string token)
        {
            return await dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == token);
        }

        /// <summary>
        /// Validates the specified refresh token and returns the associated user if the token is valid and not expired
        /// or revoked.
        /// </summary>
        /// <remarks>If the token is valid, it is revoked as part of the validation process to prevent
        /// reuse. This method updates the database to reflect the token's revoked status.</remarks>
        /// <param name="token">The refresh token string to validate. Cannot be null.</param>
        /// <returns>A <see cref="User"/> object associated with the valid token; otherwise, <see langword="null"/> if the token
        /// is invalid, expired, or revoked.</returns>
        public async Task<User?> ValidateAsync(string token)
        {
            var refreshToken = await dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Id == token);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
                return null;

            refreshToken.IsRevoked = true;
            dbContext.RefreshTokens.Update(refreshToken);
            await dbContext.SaveChangesAsync();

            return refreshToken.User;
        }

        /// <summary>
        /// Revokes the specified refresh token, preventing it from being used for future authentication requests.
        /// </summary>
        /// <remarks>If the specified token does not exist, no action is taken. This method is typically
        /// used to invalidate a refresh token after logout or when a security concern is detected.</remarks>
        /// <param name="token">The refresh token to revoke. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RevokeAsync(string token)
        {
            var refreshToken = await GetAsync(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                dbContext.RefreshTokens.Update(refreshToken);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes the refresh token associated with the specified token value, if it exists.
        /// </summary>
        /// <remarks>If the specified token does not exist, no action is taken. This method does not throw
        /// an exception if the token is not found.</remarks>
        /// <param name="token">The value of the refresh token to delete. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteAsync(string token)
        {
            var refreshToken = await GetAsync(token);
            if (refreshToken != null)
            {
                dbContext.RefreshTokens.Remove(refreshToken);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
