namespace Dpl.B2b.Contracts.Localizable

{
    public abstract class ErrorBase : LocalizableMessageBase
    {
        public override LocalizableMessageType Type => LocalizableMessageType.Error;
    }
}