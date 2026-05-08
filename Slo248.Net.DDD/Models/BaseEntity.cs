// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public abstract class BaseEntity<TId, TEntity>
    where TId : notnull
    where TEntity : notnull
{
    /// <summary>
    /// Strongly-typed identifier for this entity.
    /// </summary>
    public EntityId<TId, TEntity> Id { get; }

    /// <summary>
    /// Initializes the entity with the given <see cref="EntityId{TId, TEntity}"/>.
    /// </summary>
    /// <param name="id">The strongly-typed identifier for the entity.</param>
    protected BaseEntity(EntityId<TId, TEntity> id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }

    /// <summary>
    /// Subclasses use this to raise domain events. Concrete entities normally propagate events to their
    /// parent aggregate root by implementing this method to call the provided callback.
    /// </summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected abstract void RaiseDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Hook for applying a domain event to the entity. The default implementation throws because
    /// entities should not be applied directly via this base method; aggregates often expose their
    /// own event application logic.
    /// </summary>
    /// <param name="domainEvent">The domain event being applied.</param>
    public virtual void OnDomainEvent(IDomainEvent domainEvent)
        => throw new InvalidOperationException("This method should never be called");

    /// <summary>
    /// Determines equality based on the entity identifier only (identity-based equality).
    /// </summary>
    public bool Equals(BaseEntity<TId, TEntity>? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity<TId, TEntity> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntity<TId, TEntity>? left, BaseEntity<TId, TEntity>? right)
    {
        return left is not null && left.Equals(right);
    }

    public static bool operator !=(BaseEntity<TId, TEntity>? left, BaseEntity<TId, TEntity>? right)
    {
        return !(left == right);
    }
}
