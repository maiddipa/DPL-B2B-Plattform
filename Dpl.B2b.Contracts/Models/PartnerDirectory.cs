using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class PartnerDirectory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Partners { get; set; }
    }

    public class PartnerDirectoriesSearchRequest : PaginationRequest
    {
        public string Name { get; set; }
    }

    public class PartnerDirectoriesCreateRequest : PaginationRequest
    {
        public string Name { get; set; }
    }

    public class PartnerDirectoriesUpdateRequest : PaginationRequest
    {
        public string Name { get; set; }
    }
    
    public class PartnerDirectoryResult
    {
        public List<PartnerDirectory> PartnerDirectories { get; set; }
        public List<CustomerPartner> Partners { get; set; }
    }
}