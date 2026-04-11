// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Net.DomainDrivenDesign.Services;

public interface IDomainEventPublisher
{
    Task Publish(IDomainEvent[] events, CancellationToken cancellationToken = default);
}
