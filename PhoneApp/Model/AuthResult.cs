using System;

namespace PhoneApp.Model
{
    public class AuthResult
    {
        public AuthResult()
        {
            AquiredAt = DateTime.UtcNow;
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }

        public DateTime AquiredAt { get; set; }

        public DateTime ExpiresAt
        {
            get
            {
                // subtract a minute from the expiration force refresh as we approach expiration
                return AquiredAt + TimeSpan.FromSeconds(expires_in - 60);
            }
        }

        public bool IsExpired
        {
            get
            {
                return DateTime.UtcNow > ExpiresAt;
            }
        }
    }
}
