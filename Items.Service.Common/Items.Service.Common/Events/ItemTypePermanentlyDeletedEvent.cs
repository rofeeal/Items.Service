using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemTypePermanentlyDeletedEvent : BaseEvent
    {
        public ItemTypePermanentlyDeletedEvent() : base(nameof(ItemTypePermanentlyDeletedEvent))
        {
        }
    }
}
