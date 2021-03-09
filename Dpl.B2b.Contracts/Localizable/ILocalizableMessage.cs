namespace Dpl.B2b.Contracts.Localizable
{
    public interface ILocalizableMessage
    {
        LocalizableMessageType Type { get; }

        string Id { get; }

        string Meaning { get; }

        string Description { get; }
    }
}