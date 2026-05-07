// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Net.DomainDrivenDesign.Services;

public interface IRepository<TAggregateRoot, in TId> where TAggregateRoot : IAggregateRoot
{
    Task<TAggregateRoot?> Get(TId id, CancellationToken cancellationToken = default);
    Task Add(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    Task Update(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task Delete(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
}
