using NTech.Solutions.Api.Models;

namespace NTech.Solutions.Api.Services.AuthProviders
{
    public class GitHubAuthProvider : IAuthProvider
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public string Name => "github";

        public GitHubAuthProvider(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp");
        }

        public async Task<(AuthTokens Tokens, AuthUserInfo User)> ExchangeCodeAsync(string code, string redirectUri)
        {
            var form = new Dictionary<string, string>
            {
                ["client_id"] = _config["OAuth:GitHub:ClientId"],
                ["client_secret"] = _config["OAuth:GitHub:ClientSecret"],
                ["code"] = code,
                ["redirect_uri"] = redirectUri
            };

            var tokenResponse = await _http.PostAsync("https://github.com/login/oauth/access_token",
                new FormUrlEncodedContent(form));

            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            var query = System.Web.HttpUtility.ParseQueryString(tokenContent);
            var accessToken = query["access_token"];

            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var user = await _http.GetFromJsonAsync<GitHubUser>("https://api.github.com/user");
            var emails = await _http.GetFromJsonAsync<List<GitHubEmail>>("https://api.github.com/user/emails");

            var primaryEmail = emails?.FirstOrDefault(e => e.Primary)?.Email ?? emails?.FirstOrDefault()?.Email;

            return (new AuthTokens
            {
                AccessToken = accessToken
            },
            new AuthUserInfo
            {
                Provider = Name,
                ProviderUserId = user.Id.ToString(),
                Email = primaryEmail,
                DisplayName = user.Name ?? user.Login
            });
        }

        private class GitHubUser
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Name { get; set; }
        }

        private class GitHubEmail
        {
            public string Email { get; set; }
            public bool Primary { get; set; }
        }
    }
}
