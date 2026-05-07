// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

/// <summary>
/// Base class for implementing value objects in Domain-Driven Design.
/// Value objects are immutable objects that have no identity and whose equality is based on their values.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the atomic values that make up this value object for equality comparison.
    /// Derived classes should override this to return the values that define equality.
    /// </summary>
    /// <returns>An enumerable of objects representing the atomic values of this value object.</returns>
    protected abstract IEnumerable<object?> GetAtomicValues();

    /// <summary>
    /// Determines whether the specified <see cref="ValueObject"/> is equal to the current <see cref="ValueObject"/>.
    /// </summary>
    /// <param name="other">The value object to compare with the current value object.</param>
    /// <returns>
    /// <c>true</c> if the specified value object has the same type and atomic values as the current value object; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="ValueObject"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is a value object of the same type and has the same atomic values as the current value object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && Equals(other);
    }

    /// <summary>
    /// Gets the hash code for this value object based on its atomic values.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code computed from the atomic values.</returns>
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(HashCode), (hashCode, value) =>
            {
                hashCode.Add(value);
                return hashCode;
            })
            .ToHashCode();
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ValueObject"/> are equal.
    /// </summary>
    /// <param name="left">The first value object to compare.</param>
    /// <param name="right">The second value object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ValueObject"/> are not equal.
    /// </summary>
    /// <param name="left">The first value object to compare.</param>
    /// <param name="right">The second value object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}

public class ValueObject<T> : ValueObject
{
    public T Value { get; }

    protected ValueObject(T value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator T(ValueObject<T> valueObject) => valueObject.Value;
}
