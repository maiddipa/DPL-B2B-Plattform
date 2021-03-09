namespace Dpl.B2b.Contracts.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }

    public class OrganizationCreateRequest
    {
        public string Name { get; set; }

        public int AddressId { get; set; }
    }
    
    public class OrganizationUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
    }
    
    public class OrganizationDeleteRequest
    {
        public int Id { get; set; }
    }
    
    public class OrganizationScopedDataSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DomainScope Scope { get; set; }
        public int? ParentId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class OrganizationScopedDataSetsSearchRequest
    {
        public int OrganizationId { get; set; }
    }

    public enum DomainScope
    {
        Organization = 0,
        Customer = 1,
        Division = 2,
        LoadingLocation = 3
    }
}