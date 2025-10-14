namespace Tech.Application.Auth.DTOs
{
    public record RegisterCompanyRequest
    {
        public string CompanyName { get; init; }
        public int SubscriptionId { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}
