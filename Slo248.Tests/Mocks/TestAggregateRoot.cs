using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Tests.Mocks;

public class TestAggregateRoot : AggregateRoot<TestAggregateRoot>
{
    // Expose protected method for testing
    public TestAggregateRoot(EntityId<Guid, TestAggregateRoot> id) : base(id)
    {
    }

    public TestAggregateRoot()
    {
    }

    public void TestRaiseDomainEvent(IDomainEvent domainEvent)
    {
        RaiseDomainEvent(domainEvent);
    }
}
