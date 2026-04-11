// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Net.Tests.Core;

public class TestEntity : Entity<TestEntity>
{
    public TestEntity(EntityId<Guid, TestEntity> id, Action<IDomainEvent> raiseToRoot)
        : base(id, raiseToRoot)
    {
    }

    public TestEntity(Action<IDomainEvent> raiseToRoot)
        : base(raiseToRoot)
    {
    }

    public void TestRaiseDomainEvent(IDomainEvent domainEvent)
    {
        RaiseDomainEvent(domainEvent);
    }
}
