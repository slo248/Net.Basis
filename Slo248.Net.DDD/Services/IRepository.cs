// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DDD.Models;

namespace Slo248.Net.DDD.Services;

public interface IRepository<TAggregateRoot, in TId> where TAggregateRoot : IAggregateRoot
{
    /// <summary>
    /// Retrieves an aggregate by its identifier.
    /// </summary>
    /// <param name="id">The strongly-typed identifier for the aggregate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The aggregate instance or <c>null</c> if not found.</returns>
    Task<TAggregateRoot?> Get(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new aggregate to the repository and persists it.
    /// </summary>
    /// <remarks>
    /// Implementations should persist the aggregate, publish any uncommitted domain events via
    /// <see cref="IDomainEventPublisher"/>, and then call <c>MarkAsCommitted()</c> on the aggregate.
    /// </remarks>
    Task Add(TAggregateRoot aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing aggregate in the repository.
    /// </summary>
    /// <remarks>
    /// Implementations should persist changes, publish uncommitted events, and mark the aggregate as committed.
    /// </remarks>
    Task Update(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the aggregate from the repository.
    /// </summary>
    Task Delete(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
}
