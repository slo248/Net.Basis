// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public abstract class Entity<TId, TEntity> : BaseEntity<TId, TEntity>
    where TId : notnull
    where TEntity : notnull
{
    private readonly Action<IDomainEvent> _raiseToRoot;

    /// <summary>
    /// Creates a child entity that propagates domain events to its parent aggregate root.
    /// </summary>
    /// <param name="id">The strongly-typed identifier for the entity.</param>
    /// <param name="raiseToRoot">Callback used to raise events on the aggregate root.</param>
    protected Entity(EntityId<TId, TEntity> id, Action<IDomainEvent> raiseToRoot) : base(id)
    {
        ArgumentNullException.ThrowIfNull(raiseToRoot);

        _raiseToRoot = raiseToRoot;
    }

    /// <summary>
    /// Propagates the domain event to the parent aggregate root via the callback provided at construction.
    /// </summary>
    /// <param name="domainEvent">The domain event to propagate.</param>
    protected override void RaiseDomainEvent(IDomainEvent domainEvent) => _raiseToRoot(domainEvent);
}

public abstract class Entity<TEntity> : Entity<Guid, TEntity>
    where TEntity : notnull
{
    protected Entity(EntityId<Guid, TEntity> id, Action<IDomainEvent> raiseToRoot) : base(id, raiseToRoot)
    {
    }

    protected Entity(Action<IDomainEvent> raiseToRoot) : base(new EntityId<Guid, TEntity>(Guid.NewGuid()), raiseToRoot)
    {
    }
}
