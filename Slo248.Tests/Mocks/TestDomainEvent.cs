using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Tests.Mocks;

public class TestDomainEvent : IDomainEvent
{
    public string EventName { get; init; } = "TestEvent";
}
