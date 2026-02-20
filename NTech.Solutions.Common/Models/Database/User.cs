namespace NTech.Solutions.Common.Models.Database
{
    public class User : Common
    {

        private string _email = string.Empty;

        public string Email
        {
            get => _email;
            set => _email = value?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        public string? PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Relationships
        public List<Identity> Identities { get; set; } = new List<Identity>();
    }
}
