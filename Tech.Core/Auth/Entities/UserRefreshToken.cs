namespace Tech.Core.Auth.Entities
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
