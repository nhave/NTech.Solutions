namespace NTech.Solutions.Common.Models.Database
{
    public class RefreshToken : Common
    {
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string UserId { get; set; } = default!;

        // Relationships
        public User User { get; set; } = null!;
    }
}
