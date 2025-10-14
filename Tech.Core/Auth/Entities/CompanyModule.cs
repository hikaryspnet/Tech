namespace Tech.Core.Auth.Entities
{
    public class CompanyModule
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }  
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public CompanyModule() {}
    }
}
