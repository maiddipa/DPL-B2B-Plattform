namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer
{
    public class ValidDestinationAccount : ErrorBase
    {
        public override string Meaning => "ValidDestinationAccount";
        public override string Description => "Destination account does not exist or access denied";
    }
}
