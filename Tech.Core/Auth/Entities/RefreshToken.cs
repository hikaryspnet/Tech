using Tech.Core.Auth.Common;

namespace Tech.Core.Auth.Entities
{
        public class RefreshToken : Entity
        {
            public int UserId { get; private set; }
            public List<UserRefreshToken> UsersRefreshTokens { get; private set; }
            public string Token { get; private set; }
            public DateTime ExpiresAt { get; private set; }
            public DateTime? RevokedAt { get; private set; }

            private RefreshToken() { }

            public static RefreshToken Create(int userId, string token, DateTime expiresAt)
            {
            if (userId <= 0)
                    throw new ArgumentException("Invalid user ID.", nameof(userId));

                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Token cannot be empty.", nameof(token));

                return new RefreshToken
                {
                    UserId = userId,
                    Token = token,
                    ExpiresAt = expiresAt
                };
            }

            public void Revoke()
            {
                RevokedAt = DateTime.UtcNow;
                UpdateTimestamp();
            }

            public bool IsValid()
            {
                return RevokedAt == null && ExpiresAt > DateTime.UtcNow;
            }
        }
}
