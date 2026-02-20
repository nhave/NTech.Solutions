using NTech.Solutions.Common.Enums;

namespace NTech.Solutions.Common.Models.Database
{
    public class Identity : Common
    {
        public string UserId { get; set; }
        public OauthProvider Provider { get; set; }
        public string ProviderId { get; set; }

        // Relationships
        public User User { get; set; }
    }
}
