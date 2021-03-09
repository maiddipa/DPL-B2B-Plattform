namespace Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common
{
    public class BlacklistMatch : ErrorBase
    {
        public override string Id => "Errors|Common|Mismatch";
        public override string Meaning => "Mismatch";
        public override string Description => "Please only enter data on the delivering carrier or supplier in the form fields.";
    }
}
