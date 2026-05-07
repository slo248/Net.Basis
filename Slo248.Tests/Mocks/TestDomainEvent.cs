using Slo248.Net.DDD.Models;

namespace Slo248.Tests.Mocks;

public class TestDomainEvent : IDomainEvent
{
    public string EventName { get; init; } = "TestEvent";
}
