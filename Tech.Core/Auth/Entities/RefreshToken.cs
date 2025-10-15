using Tech.Core.Auth.Common;
using Tech.Core.Auth.Common.Result;

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

            public static Result<RefreshToken> Create(int userId, string token, DateTime expiresAt)
            {
                if (userId <= 0)
                    return Result<RefreshToken>.Fail("Invalid user ID.", Enums.ErrorType.Validation);

                if (string.IsNullOrWhiteSpace(token))
                    return Result<RefreshToken>.Fail("Token cannot be empty.", Enums.ErrorType.Validation);

                var createdToken = new RefreshToken
                {
                        UserId = userId,
                        Token = token,
                        ExpiresAt = expiresAt
                };

                return Result<RefreshToken>.Ok(createdToken);
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
