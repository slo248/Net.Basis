// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DDD.Models;

namespace Slo248.Net.DDD.Services;

public interface IDomainEventPublisher
{
    /// <summary>
    /// Publishes the given domain events to interested subscribers.
    /// </summary>
    /// <param name="events">Array of domain events to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>
    /// Implementations typically forward events to an in-process mediator (e.g., MediatR) or an external
    /// messaging system. Events should be published only after the repository has successfully persisted
    /// the aggregate state to avoid inconsistencies.
    /// </remarks>
    Task Publish(IDomainEvent[] events, CancellationToken cancellationToken = default);
}
