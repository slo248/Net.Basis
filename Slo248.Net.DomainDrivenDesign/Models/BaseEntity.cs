// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DomainDrivenDesign.Models;

public abstract class BaseEntity<TId, TEntity>
    where TId : notnull
    where TEntity : notnull
{
    public EntityId<TId, TEntity> Id { get; }

    protected BaseEntity(EntityId<TId, TEntity> id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }

    protected abstract void RaiseDomainEvent(IDomainEvent domainEvent);

    public virtual void OnDomainEvent(IDomainEvent domainEvent) => throw new NotImplementedException();

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
