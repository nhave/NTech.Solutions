using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace NTech.Solutions.Tests.Fixtures
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public IServiceProvider Services;
        //private PostgreSqlContainer _container;

        public DatabaseFixture()
        {
            //_container = new PostgreSqlBuilder("postgres")
            //    .Build();

            var coll = new ServiceCollection();

            //coll.AddTransient<IConfiguration>(_ =>
            //{
            //    var configs = new List<KeyValuePair<string, string?>>
            //    {
            //        new ("ConnectionStrings:PostgresDb", _container.GetConnectionString())
            //    };

            //    return new ConfigurationBuilder()
            //        .AddInMemoryCollection(configs)
            //        .Build();
            //});

            //coll.AddDbContext<AppDbContext>();

            Services = coll.BuildServiceProvider();
        }

        public async Task InitializeAsync()
        {
            //await _container.StartAsync();
            //var context = Services.GetRequiredService<AppDbContext>();
            //await context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            //await _container.StopAsync();
        }
    }
}
