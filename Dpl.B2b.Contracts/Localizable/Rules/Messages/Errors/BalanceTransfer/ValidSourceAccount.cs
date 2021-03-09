namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer
{
    public class ValidSourceAccount : ErrorBase
    {
        public override string Meaning => "ValidSourceAccount";
        public override string Description => "Source account does not exist or access denied";
    }
}
