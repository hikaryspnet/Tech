using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;

namespace Tech.Core.Auth.Entities
{
    public class Permission : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public PermissionType Type { get; private set; }
        public List<RolePermission> Roles { get; private set; } = new List<RolePermission>();

        private Permission() { }

        public static Permission Create(string name, PermissionType permissionType, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Permission name cannot be empty.", nameof(name));

            if (name.Length > 256) throw new ArgumentException("Name of Permission length caanot be longer than 256 symbols", nameof(name));

            if (description is not null &&  description.Length > 255) throw new ArgumentException("Permission`s description length caanot be longer than 255 symbols", nameof(name));

            if (!Enum.IsDefined(typeof(PermissionType), permissionType))
                throw new ArgumentException("Pemission type is not defined", nameof(permissionType));

            return new Permission
            {
                Name = name,
                Description = description,
                Type =  permissionType
            };
        }
    }
}
