using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookingsSimple
    {
        public int Id { get; set; }
        public string BookingTypeId { get; set; }
        public string BookingTypesName { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string ExtDescription { get; set; }
        public decimal? Amount { get; set; }
        public bool IncludeInBalance { get; set; }
        public bool Matched { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int AccountId { get; set; }
        public int TransactionId { get; set; }
        public int? ProcessId { get; set; }
        public int? Quantity { get; set; }
        public short ArticleId { get; set; }
        public string ArticlesName { get; set; }
        public string ArticlesDescription { get; set; }
        public short QualityId { get; set; }
        public string QualitiesName { get; set; }
        public string QualitiesDescription { get; set; }
        public string FullName { get; set; }
        public string AccountTypeId { get; set; }
        public string AccountTypesName { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerNumber { get; set; }
        public int? AddressId { get; set; }
        public string Description { get; set; }
        public string TransactionsReferencenumber { get; set; }
        public DateTime? TransactionsValuta { get; set; }
        public string TransactionsIntDescription { get; set; }
        public string TransactionsExtDescription { get; set; }
        public string TransactionTypeId { get; set; }
        public string TransactionTypesName { get; set; }
        public string TransactionTypesDescription { get; set; }
        public short? TransactionStateId { get; set; }
        public string TransactionStatesName { get; set; }
        public string TransactionStatesDescription { get; set; }
        public string ProcessesName { get; set; }
        public string ProcessesReferencenumber { get; set; }
        public short? ProcessTypeId { get; set; }
        public string ProcessesIntDescription { get; set; }
        public string ProcessesExtDescription { get; set; }
        public string ProcessTypesName { get; set; }
        public string ProcessTypesDescription { get; set; }
        public short? ProcessStateId { get; set; }
        public string ProcessStatesName { get; set; }
        public string ProcessStatesDescription { get; set; }
    }
}
