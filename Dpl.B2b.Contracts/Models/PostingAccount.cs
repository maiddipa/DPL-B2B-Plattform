using System;
using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Models
{
    public class PostingAccount
    {
        public int Id { get; set; }

        public int RefLtmsAccountId { get; set; }
        public string RefLtmsAccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string Name { get; set; }

        // TODO discuss if we need active on postinga ccount
        //public bool Active { get; set; }

        public PostingAccountType Type { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Balance> Balances { get; set; }

        public IEnumerable<PostingAccountCondition> TransferConditions { get; set; }
        public IEnumerable<PostingAccountCondition> VoucherConditions { get; set; }
        public IEnumerable<PostingAccountCondition> DemandConditions { get; set; }
        public IEnumerable<PostingAccountCondition> SupplyConditions { get; set; }
        public IEnumerable<PostingAccountCondition> PickupConditions { get; set; }
        public IEnumerable<PostingAccountCondition> DropoffConditions { get; set; }
    }
    
    public class PostingAccountAdministration
    {
        public int Id { get; set; }
        public int RefLtmsAccountId { get; set; }
        public string RefLtmsAccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string DisplayName { get; set; }
        public PostingAccountType Type { get; set; }
        public int AddressId { get; set; }
        public string AddressStreet1 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostalCode { get; set; }
    }

    public class PostingAccountOrderCondition
    {
        public int LoadCarrierId { get; set; }

        public int MinQuantity { get; set; }

        public int MaxQuantity { get; set; }
    }
    
    public class PostingAccountCondition
    {
        public int RefLtmsAccountId { get; set; }
        public PostingAccountConditionType Type { get; set; }
        public int LoadCarrierId { get; set; }
        public float LoadCarrierTypeOrder { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public decimal? Amount { get; set; }
    }

    public class PostingAccountsSearchRequest
    {
        public int? PostingAccountId { get; set; }
        public int? CustomerId { get; set; }
    }
    
    public enum PostingAccountConditionType
    {
        Undefined,
        Pickup,
        DropOff,
        Demand,
        Supply,
        Transfer,
        Voucher
    }
    public class PostingAccountConditionsSearchRequest
    {
        public int RefLtmsAccountId { get; set; }
    }
    
    public enum PostingAccountType
    {
        Normal = 0,
        SubAccount = 1
    }

    // TODO discuss if splitting Credit/Carhe is actually necessary for balances
    public class Balance
    {
        public int LoadCarrierId { get; set; }

        public DateTime LastBookingDateTime { get; set; }
        public int ProvisionalBalance { get; set; }
        public int CoordinatedBalance { get; set; }
        public int AvailableBalance { get; set; }

        public int ProvisionalCharge { get; set; }
        public int ProvisionalCredit { get; set; }

        public int InCoordinationCharge { get; set; }
        public int InCoordinationCredit { get; set; }

        public int UncoordinatedCharge { get; set; }
        public int UncoordinatedCredit { get; set; }

        public int PostingRequestBalanceCredit { get; set; }
        public int PostingRequestBalanceCharge { get; set; }

        public DateTime LatestUncoordinatedCharge { get; set; }
        public DateTime LatestUncoordinatedCredit { get; set; }
    }

    public class PostingAccountBalance : Balance
    {
        public int PostingAccountId { get; set; }
    }
    
    public class BalanceOverview
    {
        public string Name { get; set; }
        public int CoordinatedBalance { get; set; }
        public int ProvisionalCharge { get; set; }
        public int ProvisionalCredit { get; set; }
        public int UncoordinatedCharge { get; set; }
        public int UncoordinatedCredit { get; set; }
    }
    
    public class BalancesSummary
    {
        public int PostingAccountId { get; set; }
        public int LoadCarrierTypeId { get; set; }
        public IEnumerable<BalanceOverview> Balances{ get; set; }
        public BalanceOverview IntactBalance{ get; set; }
        public BalanceOverview DefectBalance { get; set; }
        public BalanceOverview PostingRequestBalance { get; set; }
    }

    public class BalancesSearchRequest
    {
        public int PostingAccountId { get; set; }
        public int LoadCarrierTypeId { get; set; }
        public int RefLtmsArticleId { get; set; }

        public bool ForceBalanceCalculation { get; set; }
    }

    public class AllowedPostingAccount
    {
        public int PostingAccountId { get; set; }
        public string DisplayName { get; set; }
    }

    public class PostingAccountsCreateRequest
    {
        public PostingAccountType Type { get; set; }
        public string DisplayName { get; set; }
        public int RefLtmsAccountId { get; set; }
        public string RefLtmsAccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int CustomerId { get; set; }
        public int? AddressId { get; set; }
        public int? PartnerId { get; set; }
        public int? ParentId { get; set; }
    }

    public class PostingAccountsUpdateRequest
    {
        public int Id { get; set; }
        public PostingAccountType Type { get; set; }
        public string DisplayName { get; set; }
        public int RefLtmsAccountId { get; set; }
        public string RefLtmsAccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int? AddressId { get; set; }
        public int? PartnerId { get; set; }
        public int? ParentId { get; set; }
    }
    
    public class PostingAccountsDeleteRequest
    {
        public int Id { get; set; }
    }

    public class LtmsAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
    }
}
