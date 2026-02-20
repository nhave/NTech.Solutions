using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Services
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(string id);
    }
}
