using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Common.Result;

namespace Tech.Core.Auth.Entities
{
    public class Company : Entity
    {
        private int _subscriptionId;
        private int? _companyAdminId;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public List<User> Employees { get; private set; } = [];
        public User? CompanyAdmin { get; private set; }
        public List<CompanyModule> CompaniesModules { get; set; } = new();
        public Subscription Subscription { get; private set; }

        private Company() { }

        public int SubscriptionId 
        { 
            get => _subscriptionId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("SubscriptionId must be greater than zero.", nameof(value));

                _subscriptionId = value;
            }
        }

        public int? CompanyAdminId
        {
            get => _companyAdminId;
            set
            {
                if (value is not null && value <= 0)
                    throw new ArgumentException("CompanyAdminId must be greater than zero.", nameof(value));

                _companyAdminId = value;
            }
        }
        public static Result<Company> Create(string name, string email, int subscriptionId, int adminId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Company>.Fail("Name cannot be empty.", Enums.ErrorType.Validation);

            if (name.Length > 256)
                return Result<Company>.Fail("Name length cannot be longer than 255 symbols.", Enums.ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(email))
                return Result<Company>.Fail("Email cannot be empty.", Enums.ErrorType.Validation);

            if (email.Length > 256)
                return Result<Company>.Fail("Email length cannot be longer than 255 symbols.", Enums.ErrorType.Validation);

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result<Company>.Fail("Invalid email format.", Enums.ErrorType.Validation);

            Company createdCompany = new Company
            {
                Name = name,
                Email = email,
                //CompanyAdminId = adminId == 0 ? null : subscriptionId,
                SubscriptionId = subscriptionId
            };

            return Result<Company>.Ok(createdCompany);
        }
        public void Deactivate()
        {
            UpdateTimestamp();
        }
    }
}
