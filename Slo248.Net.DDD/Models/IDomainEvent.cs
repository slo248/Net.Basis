// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

/// <summary>
/// Marker interface for domain events used within the domain model.
/// </summary>
/// <remarks>
/// Domain events represent facts that have occurred in the past and are raised by aggregates or
/// entities to describe state changes. Events implementing this interface are collected as
/// uncommitted events on an <see cref="IAggregateRoot"/> and are published by the
/// repository after the aggregate is persisted.
/// </remarks>
public interface IDomainEvent
{
}
