using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;

namespace Tech.Core.Auth.Entities
{
    public class User : Entity
    {
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public bool IsAdmin { get; private set; }
        public List<UserRole> UsersRoles { get; private set; } = [];
        public List<UserModule> UsersModules { get; private set; } = [];
        public int CompanyId { get; private set; }
        public Company Company { get; private set; }
        public List<UserRefreshToken> UsersRefreshTokens { get; private set; }


        private User() { }

        public static User Create(int companyId, string email, string password, string firstName, string lastName, bool isAdmin = false)
        {
            if (companyId <= 0)
                throw new ArgumentException("CompanyId cannot be zero or less", nameof(companyId));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (email.Length > 255) throw new ArgumentException("Email length caanot be longer than 255 symbols", nameof(email));

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (firstName.Length > 100) throw new ArgumentException("First name length caanot be longer than 100 symbols", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            if (lastName.Length > 100) throw new ArgumentException("Last name length caanot be longer than 100 symbols", nameof(lastName));

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return new User
            {
                CompanyId = companyId,
                Email = email.ToLowerInvariant(),
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin
            };
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void AddRole(Role role)
        {
            if (!UsersRoles.Any(ur => ur.RoleId == role.Id))
            {
                UsersRoles.Add(new UserRole { UserId = Id, RoleId = role.Id });
                UpdateTimestamp();
            }
        }
    }
}