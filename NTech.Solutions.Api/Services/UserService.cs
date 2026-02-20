using NTech.Solutions.Api.Repositories;
using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Invalid ID");

            return await _userRepository.GetByIdAsync(id);
        }
    }
}
