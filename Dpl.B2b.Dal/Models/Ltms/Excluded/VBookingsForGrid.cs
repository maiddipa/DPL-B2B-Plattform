using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookingsForGrid
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
        public int? TransactionCancellationId { get; set; }
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
        public string DplLsNr { get; set; }
        public DateTime? DplLsDatum { get; set; }
        public string DplReNr { get; set; }
        public int? DplReDatum { get; set; }
        public string DplAuNr { get; set; }
        public DateTime? DplAuDatum { get; set; }
        public string DplAbNr { get; set; }
        public DateTime? DplAbDatum { get; set; }
        public string DplWeNr { get; set; }
        public DateTime? DplWeDatum { get; set; }
        public string DplFaNr { get; set; }
        public DateTime? DplFaDatum { get; set; }
        public string DplGuNr { get; set; }
        public DateTime? DplGuDatum { get; set; }
        public string DplVaNr { get; set; }
        public DateTime? DplVaDatum { get; set; }
        public string DplOpgNr { get; set; }
        public DateTime? DplOpgDatum { get; set; }
        public string DplLtvNr { get; set; }
        public DateTime? DplLtvBuchungsdatum { get; set; }
        public string ExtLsNr { get; set; }
        public DateTime? ExtLsDatum { get; set; }
        public string ExtReNr { get; set; }
        public DateTime? ExtReDatum { get; set; }
        public string DplDepotBelegNr { get; set; }
        public DateTime? DplDepotBelegDatum { get; set; }
        public string ExtSnstNr { get; set; }
        public DateTime? ExtSnstDatum { get; set; }
        public string ExtAbholer { get; set; }
        public string ExtAnlieferer { get; set; }
        public string ExtKfzKz { get; set; }
    }
}
