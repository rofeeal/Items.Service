using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemCategoryCreatedEvent : BaseEvent
    {
        public ItemCategoryCreatedEvent() : base(nameof(ItemCategoryCreatedEvent))
        {
        }
        // General Information
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ForeignName { get; set; }
        public Guid? BranchId { get; set; }
        public string? Notes { get; set; }
        public bool? IsActive { get; set; } = true;

        // Auditable Information
        public DateTimeOffset? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
