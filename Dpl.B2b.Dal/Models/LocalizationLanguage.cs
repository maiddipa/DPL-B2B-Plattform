using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class LocalizationLanguage
    {
        public int Id { get; set; }

        public string Locale { get; set; }

        public string Name { get; set; }

        public virtual ICollection<LocalizationText> LocalizationTexts { get; set; }
    }
}