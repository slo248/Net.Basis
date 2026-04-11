using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Net.Tests.Core;

public class TestDomainEvent : IDomainEvent
{
    public string EventName { get; init; } = "TestEvent";
}
