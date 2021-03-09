namespace Dpl.B2b.Dal.Models
{
    /// <summary>
    ///     We need a way to retrieve localizations for reporting purposes + document generation purposes
    /// </summary>
    public class LocalizationText
    {
        public int Id { get; set; }

        //TODO Add [Index(IsUnique = true)]
        public string GeneratedId { get; set; }

        public LocalizationTextStatus Status { get; set; }
        public int LocalizationItemId { get; set; }
        public virtual LocalizationItem LocalizationItem { get; set; }

        public string Text { get; set; }
    }

    public enum LocalizationTextStatus
    {
        None = 0,
        AutoTranslated = 1,
        ManuallyTranslated = 2,
        Confirmed = 3
    }
}