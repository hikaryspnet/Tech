using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;
using Tech.Core.Auth.Common.Result;

namespace Tech.Core.Auth.Entities
{
    public class Permission : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public PermissionType Type { get; private set; }
        public List<RolePermission> Roles { get; private set; } = new List<RolePermission>();

        private Permission() { }

        public static Result<Permission> Create(string name, PermissionType permissionType, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Permission>.Fail("Permission name cannot be empty.", Enums.ErrorType.Validation);

            if (name.Length > 256)
                return Result<Permission>.Fail("Name of Permission cannot be longer than 256 symbols.", Enums.ErrorType.Validation);

            if (description is not null && description.Length > 256)
                return Result<Permission>.Fail("Permission's description cannot be longer than 256 symbols.", Enums.ErrorType.Validation);

            if (!Enum.IsDefined(typeof(PermissionType), permissionType))
                return Result<Permission>.Fail("Permission type is not defined.", Enums.ErrorType.Validation);

            var createdPermission = new Permission
            {
                Name = name,
                Description = description,
                Type =  permissionType
            };

            return Result<Permission>.Ok(createdPermission);
            
        }
    }
}
