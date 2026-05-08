// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DDD.Models;

/// <summary>
/// Represents an aggregate root in the domain model.
/// </summary>
/// <remarks>
/// An <see cref="IAggregateRoot"/> is the consistency boundary for a group of related entities and value objects.
/// Implementations expose the uncommitted domain events via <see cref="GetUncommittedEvents()"/>, which the
/// repository is responsible for publishing after persisting the aggregate. Once events have been published,
/// the repository should call <see cref="MarkAsCommitted()"/> to clear the event list and avoid duplicate publication.
/// </remarks>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets the collection of domain events that have been raised by this aggregate but not yet published.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> GetUncommittedEvents();

    /// <summary>
    /// Marks all uncommitted events as committed, clearing the internal event list.
    /// </summary>
    /// <remarks>
    /// This method should be called by repositories only after they have successfully persisted the aggregate
    /// and published the events via an <see cref="Slo248.Net.DDD.Services.IDomainEventPublisher"/>.
    /// </remarks>
    void MarkAsCommitted();
}
