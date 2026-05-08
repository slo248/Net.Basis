// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public abstract class AggregateRoot<TId, TEntity> : BaseEntity<TId, TEntity>, IAggregateRoot
    where TId : notnull
    where TEntity : notnull
{
    private readonly List<IDomainEvent> _events = [];

    /// <summary>
    /// Initializes a new aggregate root with the provided identifier.
    /// </summary>
    protected AggregateRoot(EntityId<TId, TEntity> id) : base(id)
    {
    }

    /// <summary>
    /// Gets the uncommitted domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _events.AsReadOnly();

    /// <summary>
    /// Clears the internal uncommitted event list, marking events as committed.
    /// </summary>
    /// <remarks>
    /// Repositories should call this method only after persisting the aggregate and publishing the events.
    /// </remarks>
    public void MarkAsCommitted()
    {
        _events.Clear();
    }

    /// <summary>
    /// Adds a domain event to the aggregate's uncommitted event list.
    /// </summary>
    /// <param name="domainEvent">The event to add.</param>
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
