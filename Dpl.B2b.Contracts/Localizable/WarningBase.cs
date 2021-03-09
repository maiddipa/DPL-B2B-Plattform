namespace Dpl.B2b.Contracts.Localizable
{
    public abstract class WarningBase : LocalizableMessageBase
    {
        public override LocalizableMessageType Type => LocalizableMessageType.Warning;
    }

}