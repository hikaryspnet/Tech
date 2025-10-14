namespace Tech.Application.Auth.DTOs
{
    public record AuthResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
