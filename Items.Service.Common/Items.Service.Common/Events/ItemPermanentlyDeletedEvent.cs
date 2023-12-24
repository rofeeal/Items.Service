using CQRS.Core.Events;

namespace Items.Service.Common.Events
{
    public class ItemPermanentlyDeletedEvent : BaseEvent
    {
        public ItemPermanentlyDeletedEvent() : base(nameof(ItemPermanentlyDeletedEvent))
        {
        }
    }
}
