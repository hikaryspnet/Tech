using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;
using Tech.Core.Auth.Common.Result;


namespace Tech.Core.Auth.Entities
{
    public class Role : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public RoleType RoleType { get; private set; }
        public List<RolePermission> Permissions { get; private set; } = new List<RolePermission>();
        public List<UserRole> UserRole { get; private set; } = new List<UserRole>();

        private Role() { }

        public static Result<Role> Create(string name, RoleType roleType, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Role>.Fail("Role name cannot be empty.", Enums.ErrorType.Validation);

            if (name.Length > 50)
                return Result<Role>.Fail("Name of role cannot be longer than 50 symbols.", Enums.ErrorType.Validation);

            if (description is not null && description.Length > 255)
                return Result<Role>.Fail("Description length cannot be longer than 255 symbols.", Enums.ErrorType.Validation);

            if (!Enum.IsDefined(typeof(RoleType), roleType))
                return Result<Role>.Fail("Role type is not defined.", Enums.ErrorType.Validation);

            var createdRole = new Role
            {
                Name = name,
                Description = description,
                RoleType = roleType
            };

            return Result<Role>.Ok(createdRole);
        }
    }
}
