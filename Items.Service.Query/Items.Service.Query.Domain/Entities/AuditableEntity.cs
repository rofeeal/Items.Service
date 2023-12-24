
namespace Items.Service.Query.Domain.Entities
{
    public class AuditableEntity
    {
        public DateTimeOffset? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
