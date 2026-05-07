// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public abstract class AggregateRoot<TId, TEntity> : BaseEntity<TId, TEntity>, IAggregateRoot
    where TId : notnull
    where TEntity : notnull
{
    private readonly List<IDomainEvent> _events = [];

    protected AggregateRoot(EntityId<TId, TEntity> id) : base(id)
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _events.AsReadOnly();

    public void MarkAsCommitted()
    {
        _events.Clear();
    }

    protected override void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
}

public abstract class AggregateRoot<TEntity> : AggregateRoot<Guid, TEntity>
    where TEntity : notnull
{
    protected AggregateRoot(EntityId<Guid, TEntity> id) : base(id)
    {
    }

    protected AggregateRoot() : base(new EntityId<Guid, TEntity>(Guid.NewGuid()))
    {
    }
}
