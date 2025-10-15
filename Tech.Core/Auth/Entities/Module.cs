using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;
using Tech.Core.Auth.Common.Result;


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

        public static Result<Module> Create(string name, string description, ModuleType moduleType)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Module>.Fail("Module name cannot be empty.", Enums.ErrorType.Validation);

            if (name.Length > 256)
                return Result<Module>.Fail("Module name length cannot be longer than 256 symbols.", Enums.ErrorType.Validation);

            if (description is not null && description.Length > 256)
                return Result<Module>.Fail("Module description length cannot be longer than 256 symbols.", Enums.ErrorType.Validation);

            if (!Enum.IsDefined(typeof(ModuleType), moduleType))
                return Result<Module>.Fail("Module type is not defined.", Enums.ErrorType.Validation);

            var createdModule = new Module
            {
                Name = name,
                Description = description,
                ModuleType = moduleType
            };

            return Result<Module>.Ok(createdModule);
        }

    }
}
