using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemCategoryPermanentlyDeletedEvent : BaseEvent
    {
        public ItemCategoryPermanentlyDeletedEvent() : base(nameof(ItemCategoryPermanentlyDeletedEvent))
        {
        }
    }
}
