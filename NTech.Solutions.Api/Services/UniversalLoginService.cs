using NTech.Solutions.Api.Models;
using NTech.Solutions.Api.Services.AuthProviders;

namespace NTech.Solutions.Api.Services
{
    public class UniversalLoginService
    {
        private readonly IEnumerable<IAuthProvider> _providers;

        public UniversalLoginService(IEnumerable<IAuthProvider> providers)
        {
            _providers = providers;
        }

        public async Task<(AuthTokens Tokens, AuthUserInfo User)> LoginWithCodeAsync(
            string providerName, string code, string redirectUri)
        {
            var provider = _providers.First(p => p.Name == providerName);
            return await provider.ExchangeCodeAsync(code, redirectUri);
        }

    }
}
