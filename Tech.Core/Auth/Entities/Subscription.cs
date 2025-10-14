using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;

namespace Tech.Core.Auth.Entities
{
    public class Subscription : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public decimal Price { get; private set; } 
        public SubscriptionType SubscriptionType { get; private set; }
        public List<Company> Companies { get; private set; } = [];
        public List<SubscriptionModule> IncludesModules { get; private set; } = [];

        private Subscription()
        {
                
        }

        public static Subscription Create(string name, string description, decimal price, SubscriptionType subscriptionType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subscription name cannot be empty.", nameof(name));

            if (name.Length > 256)
                throw new ArgumentException("Subscription name cannot exceed 256 characters.", nameof(name));

            if (description != null && description.Length > 256)
                throw new ArgumentException("Subscription description cannot exceed 256 characters.", nameof(description));

            if (price < 0.01m || price > 99999999999999.99m)
                throw new ArgumentException("Price must be between 0.01 and 99999999999999.99.", nameof(price));

            string priceAsString = price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            if (!Regex.IsMatch(priceAsString, @"^\d{1,14}\.\d{2}$"))
                throw new ArgumentException("Price must have up to 14 digits before the decimal point and exactly 2 digits after.", nameof(price));

            if (!Enum.IsDefined(typeof(SubscriptionType), subscriptionType))
                throw new ArgumentException("Invalid subscription type.", nameof(subscriptionType));


            return new Subscription
            {
                Name = name,
                Description = description,
                Price = price,
                SubscriptionType = subscriptionType
            };
        }
    }
}
