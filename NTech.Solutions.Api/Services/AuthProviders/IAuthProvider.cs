using NTech.Solutions.Api.Models;

namespace NTech.Solutions.Api.Services.AuthProviders
{
    public interface IAuthProvider
    {
        string Name { get; }
        Task<(AuthTokens Tokens, AuthUserInfo User)> ExchangeCodeAsync(string code, string redirectUri);
    }
}
