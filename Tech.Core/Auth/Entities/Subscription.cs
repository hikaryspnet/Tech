using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;
using Tech.Core.Auth.Common.Result;


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

        public static Result<Subscription> Create(string name, string description, decimal price, SubscriptionType subscriptionType)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Subscription>.Fail("Subscription name cannot be empty.", Enums.ErrorType.Validation);

            if (name.Length > 256)
                return Result<Subscription>.Fail("Subscription name cannot exceed 256 characters.", Enums.ErrorType.Validation);

            if (description is not null && description.Length > 256)
                return Result<Subscription>.Fail("Subscription description cannot exceed 256 characters.", Enums.ErrorType.Validation);

            if (price < 0.01m || price > 99999999999999.99m)
                return Result<Subscription>.Fail("Price must be between 0.01 and 99999999999999.99.", Enums.ErrorType.Validation);

            string priceAsString = price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            if (!Regex.IsMatch(priceAsString, @"^\\d{1,14}\\.\\d{2}$"))
                return Result<Subscription>.Fail("Price must have up to 14 digits before the decimal point and exactly 2 digits after.", Enums.ErrorType.Validation);

            if (!Enum.IsDefined(typeof(SubscriptionType), subscriptionType))
                return Result<Subscription>.Fail("Invalid subscription type.", Enums.ErrorType.Validation);


            var cratedSub = new Subscription
            {
                Name = name,
                Description = description,
                Price = price,
                SubscriptionType = subscriptionType
            };

            return Result<Subscription>.Ok(cratedSub);
        }
    }
}
