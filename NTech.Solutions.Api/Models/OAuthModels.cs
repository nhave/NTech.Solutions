namespace NTech.Solutions.Api.Models
{
    public class AuthTokens
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class AuthUserInfo
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
