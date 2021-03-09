namespace Dpl.B2b.Dal.Models
{
    public class CustomerSortingWorker
    {
        public int Id { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SortingWorker SortingWorker { get; set; }
    }
}