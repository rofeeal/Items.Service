using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemDeletedEvent : BaseEvent
    {
        public ItemDeletedEvent() : base(nameof(ItemDeletedEvent))
        {
        }
    }
}
