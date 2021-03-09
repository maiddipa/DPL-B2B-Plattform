using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class CustomerDivision
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? PostingAccountId { get; set; }
        public int? DefaultLoadingLocationId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<LoadingLocation> LoadingLocations { get; set; }
        public IEnumerable<CustomerDivisionDocumentSettings> DocumentSettings { get; set; }
    }

    public class CustomerDivisionCreateRequest
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? PostingAccountId { get; set; }
        public int? DefaultLoadingLocationId { get; set; }
        public int CustomerId { get; set; }
    }
    
    public class CustomerDivisionUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? PostingAccountId { get; set; }
        public int? DefaultLoadingLocationId { get; set; }
        public int CustomerId { get; set; }
    }
    
    public class CustomerDivisionDeleteRequest
    {
        public int Id { get; set; }
    }
}