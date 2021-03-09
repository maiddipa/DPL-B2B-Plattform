using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Authorization.Model
{
    public class OrganizationPermissionResource
    {
        public int OrganizationId { get; set; }
    }

    public class CustomerPermissionResource : OrganizationPermissionResource
    {
        public int CustomerId { get; set; }
    }

    public class DivisionPermissionResource : CustomerPermissionResource
    {
        public int DivisionId { get; set; }

        // TODO check if we need this
        public int PostingAccountId { get; set; }
    }

    public class PostingAccountPermissionResource : CustomerPermissionResource
    {
        //public int OrganizationId { get; set; }
        //public int CustomerId { get; set; }
        public int PostingAccountId { get; set; }
    }
}
