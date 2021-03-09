using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Submission : OlmaAuditable
    {
        public int Id { get; set; }

        public SubmissionType Type { get; set; }

        public SubmitterType SubmitterType { get; set; }

        public int? SubmitterUserId { get; set; }
        public virtual User SubmitterUser { get; set; }

        //ToDo: brauchen wir die SubmitterProfileId nicht?
        public int? SubmitterProfileId { get; set; }
        public virtual SubmitterProfile SubmitterProfile { get; set; }

        public int PostingAccountId { get; set; }
        public virtual PostingAccount PostingAccount { get; set; }

        public virtual ICollection<PostingRequest> PostingRequests { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
        public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; }
    }

    public enum SubmissionType
    {
        Voucher = 0,
        DeliveryNote = 1
    }

    public enum SubmitterType
    {
        Authenticated = 0,

        // TODO We need to discuss how to handle this moving forward. We need to be able to always identify users, maybe only ask for the email address and send an authenticated link
        Anonymous = 1
    }

    // TODO decide if it would make sense to create a user or person instead or partner
    public class SubmitterProfile : OlmaAuditable
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        // TODO add address information
        // TODO add contact information
    }
}