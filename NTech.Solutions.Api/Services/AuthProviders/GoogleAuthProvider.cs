using NTech.Solutions.Api.Models;
using System.IdentityModel.Tokens.Jwt;

namespace NTech.Solutions.Api.Services.AuthProviders
{
    public class GoogleAuthProvider : IAuthProvider
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public string Name => "google";

        public GoogleAuthProvider(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<(AuthTokens Tokens, AuthUserInfo User)> ExchangeCodeAsync(
            string code, string redirectUri)
        {
            var clientId = _config["OAuth:Google:ClientId"];
            var clientSecret = _config["OAuth:Google:ClientSecret"];

            // 1. Udveksl authorization code for tokens
            var form = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code"
            };

            var response = await _http.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(form));

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<GoogleTokenResponse>();

            // 2. Dekod ID Token (JWT)
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenResponse.id_token);

            // 3. Normaliser brugerinfo
            var user = new AuthUserInfo
            {
                Provider = Name,
                ProviderUserId = jwt.Claims.First(c => c.Type == "sub").Value,
                Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                DisplayName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                FirstName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                LastName = jwt.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value
            };

            // 4. Returnér tokens + brugerinfo
            var tokens = new AuthTokens
            {
                AccessToken = tokenResponse.access_token,
                IdToken = tokenResponse.id_token,
                RefreshToken = tokenResponse.refresh_token
            };

            return (tokens, user);
        }

        private class GoogleTokenResponse
        {
            public string access_token { get; set; }
            public string id_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }
    }
}
