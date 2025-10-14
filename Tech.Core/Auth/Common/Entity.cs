namespace Tech.Core.Auth.Common
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public DateTime? UpdatedAt { get; protected set; }

        protected Entity() { CreatedAt = DateTime.UtcNow; }

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
