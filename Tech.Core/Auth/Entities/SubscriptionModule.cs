
namespace Tech.Core.Auth.Entities
{
    public class SubscriptionModule
    {
        public int SubscriptionId {  get; set; }
        public Subscription Subscription { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
