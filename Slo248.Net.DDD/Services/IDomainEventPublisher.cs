// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DDD.Models;

namespace Slo248.Net.DDD.Services;

public interface IDomainEventPublisher
{
    Task Publish(IDomainEvent[] events, CancellationToken cancellationToken = default);
}
