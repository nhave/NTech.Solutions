using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
        /// specified identifier, or <see langword="null"/> if no user is found.</returns>
        public Task<User?> GetByIdAsync(string id);

        /// <summary>
        /// Asynchronously retrieves a collection of users.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="User"/> objects representing the users. The collection will be empty if no users are found.</returns>
        public Task<ICollection<User>> GetUsersAsync();

        /// <summary>
        /// Asynchronously retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
        /// specified email address, or null if no user is found.</returns>
        public Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Asynchronously adds the specified user to the collection.
        /// </summary>
        /// <param name="user">The user to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public Task Add(User user);

        /// <summary>
        /// Creates a new user account asynchronously using the specified email address and password hash.
        /// </summary>
        /// <param name="email">The email address to associate with the new user. Cannot be null or empty.</param>
        /// <param name="passwordHash">The hashed password for the new user. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created user if successful;
        /// otherwise, null if the user could not be created.</returns>
        public Task<User?> CreateAsync(string email, string passwordHash);

        /// <summary>
        /// Asynchronously removes the item with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to remove. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        public Task RemoveAsync(string id);

        /// <summary>
        /// Asynchronously saves all changes made in the context to the underlying data store.
        /// </summary>
        /// <remarks>This method commits all tracked changes to the data store. If there are no changes,
        /// no updates are performed. Await the returned task to ensure that the operation completes before
        /// proceeding.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public Task SaveChangesAsync();
    }
}
