using CQRS.Core.Domain;
using Items.Service.Common.Events;

namespace Items.Service.Cmd.Domain.Aggregates
{
    public class ItemTypeAggregate : AggregateRoot
    {
        public string? _code;
        public string? _name;
        public string? _foreignName;
        public string? _notes;
        public Guid? _branchId;
        public bool? _isActive = true;


        public ItemTypeAggregate()
        {
        }

        public ItemTypeAggregate(Guid id, string? code, string? name, string? foreignName, string? notes, Guid? branchId, bool? isActive)
        {
            RaiseEvent(new ItemTypeCreatedEvent
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

        public void Apply(ItemTypeCreatedEvent @event)
        {
            _id = @event.Id;
            _code = @event.Code;
            _name = @event.Name;
            _foreignName = @event.ForeignName;
            _branchId = @event.BranchId;
            _notes = @event.Notes;
            _isActive = @event.IsActive;
        }
        public void EditItemTypeAggregate(Guid id, string? code, string? name, string? foreignName, Guid? branchId, string? notes, bool? isActive)
        {
            RaiseEvent(new ItemTypeEditedEvent
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

        public void Apply(ItemTypeEditedEvent @event)
        {
            _id = @event.Id;
            _code = @event.Code;
            _name = @event.Name;
            _foreignName = @event.ForeignName;
            _branchId = @event.BranchId;
            _notes = @event.Notes;
            _isActive = @event.IsActive;
        }

        public void DeleteItemType(Guid id)
        {
            RaiseEvent(new ItemTypeDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemTypeDeletedEvent @event)
        {
            _id = @event.Id;
        }

        public void DeleteItemTypePermanently(Guid id)
        {
            RaiseEvent(new ItemTypePermanentlyDeletedEvent
            {
                Id = id
            });
        }

        public void Apply(ItemTypePermanentlyDeletedEvent @event)
        {
            _id = @event.Id;
        }

    }
}
