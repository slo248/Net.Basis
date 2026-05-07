// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DDD.Models;
using Slo248.Tests.Mocks;

namespace Slo248.Tests.Domain;

public class AggregateRootTests
{
    [Fact]
    public void Constructor_WithValidId_ShouldInitializeSuccessfully()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());

        // Act
        var aggregateRoot = new TestAggregateRoot(id);

        // Assert
        Assert.NotNull(aggregateRoot);
        Assert.Equal(id, aggregateRoot.Id);
    }

    [Fact]
    public void Constructor_WithNullId_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestAggregateRoot(null!));
    }

    [Fact]
    public void GetUncommittedEvents_WhenNoEventsRaised_ShouldReturnEmptyCollection()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);

        // Act
        var events = aggregateRoot.GetUncommittedEvents();

        // Assert
        Assert.NotNull(events);
        Assert.Empty(events);
    }

    [Fact]
    public void RaiseDomainEvent_WithValidEvent_ShouldAddEventToUncommittedEvents()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);
        var domainEvent = new TestDomainEvent { EventName = "UserCreated" };

        // Act
        aggregateRoot.TestRaiseDomainEvent(domainEvent);

        // Assert
        var events = aggregateRoot.GetUncommittedEvents();
        Assert.Single(events);
        Assert.Contains(domainEvent, events);
    }

    [Fact]
    public void RaiseDomainEvent_WithMultipleEvents_ShouldAddAllEventsToUncommittedEvents()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);
        var event1 = new TestDomainEvent { EventName = "Event1" };
        var event2 = new TestDomainEvent { EventName = "Event2" };
        var event3 = new TestDomainEvent { EventName = "Event3" };

        // Act
        aggregateRoot.TestRaiseDomainEvent(event1);
        aggregateRoot.TestRaiseDomainEvent(event2);
        aggregateRoot.TestRaiseDomainEvent(event3);

        // Assert
        var events = aggregateRoot.GetUncommittedEvents();
        Assert.Equal(3, events.Count);
        Assert.Contains(event1, events);
        Assert.Contains(event2, events);
        Assert.Contains(event3, events);
    }

    [Fact]
    public void MarkAsCommitted_ShouldClearUncommittedEvents()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);
        aggregateRoot.TestRaiseDomainEvent(new TestDomainEvent());
        aggregateRoot.TestRaiseDomainEvent(new TestDomainEvent());

        // Act
        aggregateRoot.MarkAsCommitted();

        // Assert
        var events = aggregateRoot.GetUncommittedEvents();
        Assert.Empty(events);
    }

    [Fact]
    public void MarkAsCommitted_WhenNoEventsRaised_ShouldNotThrowException()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);

        // Act & Assert
        aggregateRoot.MarkAsCommitted(); // Should not throw
    }

    [Fact]
    public void RaiseDomainEvent_AfterMarkAsCommitted_ShouldTrackNewEvents()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);
        var event1 = new TestDomainEvent { EventName = "Event1" };
        aggregateRoot.TestRaiseDomainEvent(event1);
        aggregateRoot.MarkAsCommitted();

        var event2 = new TestDomainEvent { EventName = "Event2" };

        // Act
        aggregateRoot.TestRaiseDomainEvent(event2);

        // Assert
        var events = aggregateRoot.GetUncommittedEvents();
        Assert.Single(events);
        Assert.Contains(event2, events);
        Assert.DoesNotContain(event1, events);
    }

    [Fact]
    public void Constructor_WithDefaultGuidOverload_ShouldUseGuidAsDefaultId()
    {
        // Arrange
        var guidId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());

        // Act
        var aggregateRoot = new TestAggregateRoot(guidId);

        // Assert
        Assert.NotNull(aggregateRoot);
        Assert.IsType<Guid>(aggregateRoot.Id.Value);
    }

    [Fact]
    public void Constructor_WithoutParameters_ShouldGenerateNewGuidId()
    {
        // Act
        var aggregateRoot1 = new TestAggregateRoot();
        var aggregateRoot2 = new TestAggregateRoot();

        // Assert
        Assert.NotNull(aggregateRoot1);
        Assert.NotNull(aggregateRoot2);
        Assert.NotEqual(aggregateRoot1.Id.Value, aggregateRoot2.Id.Value);
        Assert.IsType<Guid>(aggregateRoot1.Id.Value);
        Assert.IsType<Guid>(aggregateRoot2.Id.Value);
    }

    [Fact]
    public void Equals_WithSameId_ShouldBeEqual()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(sharedId);
        var aggregateRoot2 = new TestAggregateRoot(sharedId);

        // Act & Assert
        Assert.Equal(aggregateRoot1, aggregateRoot2);
    }

    [Fact]
    public void Equals_WithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var id1 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var id2 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(id1);
        var aggregateRoot2 = new TestAggregateRoot(id2);

        // Act & Assert
        Assert.NotEqual(aggregateRoot1, aggregateRoot2);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var id = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot = new TestAggregateRoot(id);

        // Act & Assert
        Assert.NotEqual(aggregateRoot, null);
    }

    [Fact]
    public void EqualityOperator_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(sharedId);
        var aggregateRoot2 = new TestAggregateRoot(sharedId);

        // Act & Assert
        Assert.True(aggregateRoot1 == aggregateRoot2);
    }

    [Fact]
    public void InequalityOperator_WithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var id1 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var id2 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(id1);
        var aggregateRoot2 = new TestAggregateRoot(id2);

        // Act & Assert
        Assert.True(aggregateRoot1 != aggregateRoot2);
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldBeSame()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(sharedId);
        var aggregateRoot2 = new TestAggregateRoot(sharedId);

        // Act & Assert
        Assert.Equal(aggregateRoot1.GetHashCode(), aggregateRoot2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentIds_ShouldBeDifferent()
    {
        // Arrange
        var id1 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var id2 = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(id1);
        var aggregateRoot2 = new TestAggregateRoot(id2);
        // Act & Assert
        Assert.NotEqual(aggregateRoot1.GetHashCode(), aggregateRoot2.GetHashCode());
    }

    [Fact]
    public void AggregateRoots_WithSameIdCanBeUsedInCollection()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(sharedId);
        var aggregateRoot2 = new TestAggregateRoot(sharedId);
        var hashSet = new HashSet<TestAggregateRoot> { aggregateRoot1 };

        // Act & Assert
        // Since they have the same ID and same hash code, hashSet should consider them equal
        Assert.Contains(aggregateRoot2, hashSet);
    }

    [Fact]
    public void Equality_ShouldIgnoreDifferentDomainEvents()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregateRoot1 = new TestAggregateRoot(sharedId);
        var aggregateRoot2 = new TestAggregateRoot(sharedId);

        // One has events, the other doesn't
        aggregateRoot1.TestRaiseDomainEvent(new TestDomainEvent());

        // Act & Assert
        // They should still be equal because equality is based on ID, not events
        Assert.Equal(aggregateRoot1, aggregateRoot2);
    }
}
