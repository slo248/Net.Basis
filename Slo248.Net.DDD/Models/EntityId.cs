// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

public class EntityId<TId, TEntity> : IEquatable<EntityId<TId, TEntity>>
    where TId : notnull
    where TEntity : notnull
{
    /// <summary>
    /// Strongly-typed identifier wrapper that pairs a primitive id type with an entity type to prevent
    /// accidental mixing of identifiers at compile time.
    /// </summary>
    /// <remarks>
    /// Use <see cref="EntityId{TEntity}"/> for the common Guid-based identifiers. Implicit conversions
    /// exist from <see cref="EntityId{TId, TEntity}"/> to <typeparamref name="TId"/>, and from
    /// <see cref="EntityId{TId, TEntity}"/> to <see cref="EntityId{TEntity}"/> when the underlying
    /// <typeparamref name="TId"/> is a <see cref="Guid"/>.
    /// </remarks>
    public EntityId(TId value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    /// <summary>
    /// The underlying identifier value.
    /// </summary>
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

    /// <summary>
    /// Implicit conversion to <see cref="EntityId{TEntity}"/> for Guid-based ids.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the underlying id is not a Guid.</exception>
    public static implicit operator EntityId<TEntity>(EntityId<TId, TEntity> id)
    {
        if (id.Value is Guid guid)
        {
            return new EntityId<TEntity>(guid);
        }

        throw new InvalidOperationException("EntityId must be a Guid");
    }

    /// <summary>
    /// Implicit conversion to the underlying id type <typeparamref name="TId"/>.
    /// </summary>
    public static implicit operator TId(EntityId<TId, TEntity> id) => id.Value;
}

public class EntityId<TEntity> : EntityId<Guid, TEntity>
    where TEntity : notnull
{
    /// <summary>
    /// Shortcut strongly-typed identifier for the common Guid-based identifiers.
    /// </summary>
    /// <param name="value">The Guid value for the identifier.</param>
    public EntityId(Guid value) : base(value)
    {
    }
}
