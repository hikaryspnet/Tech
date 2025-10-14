using Tech.Core.Auth.Common;
using Tech.Core.Auth.Enums;

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

        public static Role Create(string name, RoleType roleType, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty.", nameof(name));

            if (name.Length > 50) throw new ArgumentException("Name of role length caanot be longer than 50 symbols", nameof(name));

            if (description is not null && description.Length > 255) throw new ArgumentException("Description length caanot be longer than 255 symbols", nameof(description));

            if (!Enum.IsDefined(typeof(RoleType), roleType)) throw new ArgumentException("Roll type is not defined", nameof(RoleType));
            
            return new Role
            {
                Name = name,
                Description = description,
                RoleType = roleType
            };
        }

        //public void AddPermission(Permission permission)
        //{
        //    if (!Permissions.Any(rp => rp.PermissionId == permission.Id))
        //    {
        //        Permissions.Add(new RolePermission { RoleId = Id, PermissionId = permission.Id });
        //        UpdateTimestamp();
        //    }
        //}
    }
}
