using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;

namespace Tech.Core.Auth.Entities
{
    public class Company : Entity
    {
        private int _subscriptionId;
        private int? _companyAdminId;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public List<User> Employees { get; private set; } = [];
        //public int? CompanyAdminId { get;private set;}
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
        public static Company Create(string name, string email, int subscriptionId, int adminId)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (name.Length > 256) 
                throw new ArgumentException("Name length caanot be longer than 255 symbols", nameof(name));


            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (email.Length > 256) 
                throw new ArgumentException("Email length caanot be longer than 255 symbols");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            return new Company
            {
                Name = name,
                Email = email,
                CompanyAdminId = adminId == 0 ? null : subscriptionId,
                SubscriptionId = subscriptionId,
            };
        }
        public void Deactivate()
        {
            UpdateTimestamp();
        }
    }
}
