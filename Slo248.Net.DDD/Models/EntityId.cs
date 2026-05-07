// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public sealed class EntityId<TId, TEntity> : IEquatable<EntityId<TId, TEntity>>
    where TId : notnull
    where TEntity : notnull
{
    public EntityId(TId value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public TId Value { get; }

    public bool Equals(EntityId<TId, TEntity>? other)
    {
        return other is not null && EqualityComparer<TId>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is EntityId<TId, TEntity> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value.ToString() ?? string.Empty;
    }

    public static bool operator ==(EntityId<TId, TEntity>? left, EntityId<TId, TEntity>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EntityId<TId, TEntity>? left, EntityId<TId, TEntity>? right)
    {
        return !Equals(left, right);
    }
}
