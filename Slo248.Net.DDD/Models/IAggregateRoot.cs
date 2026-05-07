// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.DomainDrivenDesign.Models;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetUncommittedEvents();
    void MarkAsCommitted();
}
