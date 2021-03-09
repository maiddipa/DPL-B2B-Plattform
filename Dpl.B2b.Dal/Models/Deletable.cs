namespace Dpl.B2b.Dal.Models
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }
    }

    public class Deletable : IDeletable
    {
        public bool IsDeleted { get; set; }
    }
}