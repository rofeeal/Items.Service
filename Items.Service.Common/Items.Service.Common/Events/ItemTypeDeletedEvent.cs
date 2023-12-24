using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemTypeDeletedEvent : BaseEvent
    {
        public ItemTypeDeletedEvent() : base(nameof(ItemTypeDeletedEvent))
        {
        }
    }
}
