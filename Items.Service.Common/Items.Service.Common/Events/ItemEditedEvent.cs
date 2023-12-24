using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemEditedEvent : BaseEvent
    {
        public ItemEditedEvent() : base(nameof(ItemEditedEvent))
        {
        }
        public string? Code { get; set; }
		public string Name { get; set; }
		public string ForeignName { get; set; }
		public Guid? TypeId { get; set; }
		public Guid? CategoryId { get; set; }
		public Guid? UnitId { get; set; }
		public Guid? SupplierId { get; set; }
		public decimal Price { get; set; }
		public decimal Cost { get; set; }
		public Guid? TaxCodeTypeId { get; set; }
		public string? TaxCode { get; set; }
		public bool IsForPurchase { get; set; }
		public bool IsForSale { get; set; }
		public string? Notes { get; set; }
		public bool IsActive { get; set; }
    }
}
