using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;

namespace Tech.Core.Auth.Entities
{
    public class Module : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public ModuleType ModuleType { get; private set; }
        public List<CompanyModule> CompaniesModules { get; set; } = [];
        public List<SubscriptionModule> SubscriptionsModules { get; set; } = [];
        public List<UserModule> UsersModules { get; set; } = [];

        private Module() { }

        public static Module Create(string name, string description, ModuleType moduleType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Module name cannot be empty.", nameof(name));
            if (name.Length > 256)
                throw new ArgumentException("Module name length caanot be longer than 256 symbols.", nameof(name));


            if (description is not null && description.Length > 256)
                throw new ArgumentException("Module description length caanot be longer than 256 symbols", nameof(description));

            if(!Enum.IsDefined(typeof(ModuleType), moduleType)) 
                throw new ArgumentException("Module type is not defined" ,nameof(moduleType));

            return new Module
            {
                Name = name,
                Description = description,
                ModuleType = moduleType
            };
        }

    }
}
