namespace Tech.Core.Auth.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Fingerprint { get; set; } = default!;
        public string DeviceType { get; set; } = default!; 
        public string OperatingSystem { get; set; } = default!;
        public string Browser { get; set; } = default!;
        public string IpAddress { get; set; } = default!;
        public DateTime LastUsed { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = default!;

        private Device() {}

        public Device Create()
        {
            return new Device() { };
        }
    }
}
