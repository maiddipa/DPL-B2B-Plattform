using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Algorithm;

namespace Dpl.B2b.Dal.Models
{
    public class LocalizationItem
    {
        public int Id { get; set; }

        public LocalizationItemType Type { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// For Different Translation meaning or context type
        /// </summary>
        [StringLength(254)]
        public string FieldName { get; set; }
    }

    public enum LocalizationItemType
    {
        Entity = 0,
        Enum = 1,
    }

    // LTMS.ProcessType (Name + Description)
    //
}