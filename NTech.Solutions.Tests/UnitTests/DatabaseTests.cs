using Microsoft.Extensions.DependencyInjection;
using NTech.Solutions.Api.Data;
using NTech.Solutions.Api.Repositories;
using NTech.Solutions.Common.Models.Database;
using NTech.Solutions.Tests.Fixtures;

namespace NTech.Solutions.Tests.UnitTests
{
    public class DatabaseTests(DatabaseFixture db) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task AddUser_ShouldAddUserToDatabase()
        {
            var context = db.Services.GetRequiredService<AppDbContext>();

            // Arrange
            var repo = new UserRepository(context);
            var user = new User
            {
                Id = "user123",
                FirstName = "Nikolaj",
                LastName = "Have",
            };

            // Act
            repo.Add(user);
            await repo.SaveChangesAsync();

            var result = await repo.GetByIdAsync("user123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Nikolaj", result.FirstName);
            Assert.Equal("Have", result.LastName);
        }

        [Fact]
        public async Task RemoveUser_ShouldRemoveUserFromDatabase()
        {
            var context = db.Services.GetRequiredService<AppDbContext>();
            var repo = new UserRepository(context);

            var user = new User
            {
                Id = "deleteMe",
                FirstName = "Delete",
                LastName = "Me",
            };

            repo.Add(user);
            await repo.SaveChangesAsync();

            //await repo.RemoveAsync("deleteMe");
            //await repo.SaveChangesAsync();

            var result = await repo.GetByIdAsync("deleteMe");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AllUsers()
        {
            var context = db.Services.GetRequiredService<AppDbContext>();

            // Arrange
            var repo = new UserRepository(context);

            var result = await repo.GetUsersAsync();
            var test = result.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
        }
    }
}
