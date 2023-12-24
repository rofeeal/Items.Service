using Items.Service.Common.Events;

namespace Items.Service.Query.Infrastructure.Interfaces
{
    public interface IEventHandler
    {
        Task On(ItemCreatedEvent @event);
        Task On(ItemEditedEvent @event);
        Task On(ItemDeletedEvent @event);
        Task On(ItemPermanentlyDeletedEvent @event);

        Task On(ItemTypeCreatedEvent @event);
        Task On(ItemTypeEditedEvent @event);
        Task On(ItemTypeDeletedEvent @event);
        Task On(ItemTypePermanentlyDeletedEvent @event);

		Task On(ItemCategoryCreatedEvent @event);
		Task On(ItemCategoryEditedEvent @event);
		Task On(ItemCategoryDeletedEvent @event);
		Task On(ItemCategoryPermanentlyDeletedEvent @event);
	}
}
