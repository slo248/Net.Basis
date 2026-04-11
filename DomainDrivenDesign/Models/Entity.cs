// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace DomainDrivenDesign.Models;

public abstract class Entity<TId, TEntity>: BaseEntity<TId, TEntity>
    where TId: notnull
    where TEntity: notnull
{
    private readonly Action<IDomainEvent> _raiseToRoot;

    protected Entity(EntityId<TId, TEntity> id, Action<IDomainEvent> raiseToRoot) : base(id)
    {
        _raiseToRoot = raiseToRoot;
    }

    protected override void RaiseDomainEvent(IDomainEvent domainEvent) => _raiseToRoot(domainEvent);
}

public abstract class Entity<TEntity>: Entity<Guid, TEntity>
    where TEntity: notnull
{
    protected Entity(EntityId<Guid, TEntity> id, Action<IDomainEvent> raiseToRoot) : base(id, raiseToRoot)
    {
    }
}
