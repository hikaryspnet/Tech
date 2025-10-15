using System.Text.RegularExpressions;
using Tech.Core.Auth.Common;
using Tech.Core.Auth.Common.Result;


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
        public List<Device> Devices { get; set; } = new List<Device>();


        private User() { }

        public static Result<User> Create(int companyId, string email, string password, string firstName, string lastName, bool isAdmin = false)
        {
            if (companyId <= 0)
                return Result<User>.Fail("CompanyId cannot be zero or less.", Enums.ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(email))
                return Result<User>.Fail("Email cannot be empty.", Enums.ErrorType.Validation);

            if (email.Length > 255)
                return Result<User>.Fail("Email length cannot be longer than 255 symbols.", Enums.ErrorType.Validation);

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result<User>.Fail("Invalid email format.", Enums.ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(password))
                return Result<User>.Fail("Password cannot be empty.", Enums.ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(firstName))
                return Result<User>.Fail("First name cannot be empty.", Enums.ErrorType.Validation);

            if (firstName.Length > 100)
                return Result<User>.Fail("First name length cannot be longer than 100 symbols.", Enums.ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(lastName))
                return Result<User>.Fail("Last name cannot be empty.", Enums.ErrorType.Validation);

            if (lastName.Length > 100)
                return Result<User>.Fail("Last name length cannot be longer than 100 symbols.", Enums.ErrorType.Validation);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var createdUser = new User
            {
                CompanyId = companyId,
                Email = email.ToLowerInvariant(),
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin
            };

            return Result<User>.Ok(createdUser);
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