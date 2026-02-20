using NTech.Solutions.Api.Repositories;
using NTech.Solutions.Api.Services;
using NTech.Solutions.Api.Services.AuthProviders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();

            services.AddHttpClient<GoogleAuthProvider>();
            services.AddHttpClient<GitHubAuthProvider>();

            services.AddSingleton<IAuthProvider, GoogleAuthProvider>();
            services.AddSingleton<IAuthProvider, GitHubAuthProvider>();

            services.AddSingleton<UniversalLoginService>();


            return services;
        }
    }
}

