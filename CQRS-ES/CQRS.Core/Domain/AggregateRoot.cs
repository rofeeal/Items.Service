using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        // Auditable properties
        public DateTimeOffset? CreatedOn { get; private set; }
        public Guid? CreatedBy { get; private set; }
        public DateTimeOffset? ModifiedOn { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTimeOffset? DeletedOn { get; private set; }
        public Guid? DeletedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        public Guid Id
        {
            get { return _id; }
        }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}!");
            }

            method.Invoke(this, new object[] { @event });

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }

        public void SetCreatedInfo(Guid? createdBy)
        {
            CreatedOn = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public void SetModifiedInfo(Guid? modifiedBy)
        {
            ModifiedOn = DateTimeOffset.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public void SetDeletedInfo(Guid? deletedBy)
        {
            DeletedOn = DateTimeOffset.UtcNow;
            DeletedBy = deletedBy;
            IsDeleted = true;
        }
    }
}