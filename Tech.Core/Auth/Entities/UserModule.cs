namespace Tech.Core.Auth.Entities
{
    public class UserModule
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Module Module { get; set; } 
        public int ModuleId { get; set; }
    }
}
