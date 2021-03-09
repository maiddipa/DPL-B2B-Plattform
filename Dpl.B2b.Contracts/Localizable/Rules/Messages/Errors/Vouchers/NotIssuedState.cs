namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Vouchers
{
    public class NotIssuedState : ErrorBase
    {
        public override string Meaning => "NotIssuedState";
        public override string Description => "Canceling voucher failed. Only Vouchers in 'issued' state can be canceled";
    }
}
