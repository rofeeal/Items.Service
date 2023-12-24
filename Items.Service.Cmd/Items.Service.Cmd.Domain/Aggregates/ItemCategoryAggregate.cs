using CQRS.Core.Domain;
using Items.Service.Common.Events;

namespace Items.Service.Cmd.Domain.Aggregates
{
    public class ItemCategoryAggregate : AggregateRoot
    {
        public string? _code;
        public string? _name;
        public string? _foreignName;
        public string? _notes;
        public Guid? _branchId;
        public bool? _isActive = true;


        public ItemCategoryAggregate()
        {
        }

        public ItemCategoryAggregate(Guid id, string? code, string? name, string? foreignName, string? notes, Guid? branchId, bool? isActive)
        {
            RaiseEvent(new ItemCategoryCreatedEvent
            {
                Id = id,
                Code = code,
                Name = name,
                ForeignName = foreignName,
                Notes = notes,
                BranchId = branchId,
                IsActive = isActive
            });
        }

        public void Apply(ItemCategoryCreatedEvent @event)
        {
            _id = @event.Id;
            _code = @event.Code;
            _name = @event.Name;
            _foreignName = @event.ForeignName;
            _branchId = @event.BranchId;
            _notes = @event.Notes;
            _isActive = @event.IsActive;
        }
        public void EditItemCategoryAggregate(Guid id, string? code, string? name, string? foreignName, Guid? branchId, string? notes, bool? isActive)
        {
            RaiseEvent(new ItemCategoryEditedEvent
            {
                Id = id,
                Code = code,
                Name = name,
                ForeignName = foreignName,
                BranchId = branchId,
                Notes = notes,
                IsActive = isActive
            });
        }

        public void Apply(ItemCategoryEditedEvent @event)
        {
            _id = @event.Id;
            _code = @event.Code;
            _name = @event.Name;
            _foreignName = @event.ForeignName;
            _branchId = @event.BranchId;
            _notes = @event.Notes;
            _isActive = @event.IsActive;
        }

        public void DeleteItemCategory(Guid id)
        {
            RaiseEvent(new ItemCategoryDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemCategoryDeletedEvent @event)
        {
            _id = @event.Id;
        }

        public void DeleteItemCategoryPermanently(Guid id)
        {
            RaiseEvent(new ItemCategoryPermanentlyDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemCategoryPermanentlyDeletedEvent @event)
        {
            _id = @event.Id;
        }

    }
}
