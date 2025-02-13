

namespace ProductCatalog_DAL.Models
{
    public class BaseEntity
    {
        public int? Id { get; set; }
        public DateTime? creationDate { get; set; } 
        public int? createdBy { get; set; }
        public DateTime? startDate { get; set; } = DateTime.Now;
        public int? duration { get; set; }
    }
}
