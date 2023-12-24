using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemCategoryDeletedEvent : BaseEvent
    {
        public ItemCategoryDeletedEvent() : base(nameof(ItemCategoryDeletedEvent))
        {
        }
    }
}
