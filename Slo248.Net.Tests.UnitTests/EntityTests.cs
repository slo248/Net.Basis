// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DomainDrivenDesign.Models;
using Slo248.Net.Tests.Core;

namespace Slo248.Net.Tests.UnitTests;

public class EntityTests
{
    [Fact]
    public void Constructor_WithValidIdAndCallback_ShouldInitializeSuccessfully()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var callbackInvoked = false;
        Action<IDomainEvent> callback = _ => callbackInvoked = true;

        // Act
        var entity = new TestEntity(id, callback);

        // Assert
        Assert.NotNull(entity);
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Constructor_WithNullId_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Action<IDomainEvent> callback = _ => { };
        Assert.Throws<ArgumentNullException>(() => new TestEntity(null!, callback));
    }

    [Fact]
    public void Constructor_WithNullCallback_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TestEntity(id, null!));
    }

    [Fact]
    public void RaiseDomainEvent_ShouldInvokeCallbackWithEvent()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var capturedEvent = (IDomainEvent?)null;
        Action<IDomainEvent> callback = @event => capturedEvent = @event;
        var entity = new TestEntity(id, callback);
        var domainEvent = new TestDomainEvent { EventName = "TestEvent" };

        // Act
        entity.TestRaiseDomainEvent(domainEvent);

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Same(domainEvent, capturedEvent);
    }

    [Fact]
    public void RaiseDomainEvent_WithMultipleEvents_ShouldInvokeCallbackForEach()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var invokedCount = 0;
        var capturedEvents = new List<IDomainEvent>();
        Action<IDomainEvent> callback = @event =>
        {
            invokedCount++;
            capturedEvents.Add(@event);
        };
        var entity = new TestEntity(id, callback);
        var event1 = new TestDomainEvent { EventName = "Event1" };
        var event2 = new TestDomainEvent { EventName = "Event2" };
        var event3 = new TestDomainEvent { EventName = "Event3" };

        // Act
        entity.TestRaiseDomainEvent(event1);
        entity.TestRaiseDomainEvent(event2);
        entity.TestRaiseDomainEvent(event3);

        // Assert
        Assert.Equal(3, invokedCount);
        Assert.Equal(3, capturedEvents.Count);
        Assert.Contains(event1, capturedEvents);
        Assert.Contains(event2, capturedEvents);
        Assert.Contains(event3, capturedEvents);
    }

    [Fact]
    public void RaiseDomainEvent_ShouldPropagateEventToAggregate()
    {
        // Arrange
        var aggregateId = new EntityId<Guid, TestAggregateRoot>(Guid.NewGuid());
        var aggregate = new TestAggregateRoot(aggregateId);
        var entityId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var entity = new TestEntity(entityId, @event => aggregate.TestRaiseDomainEvent(@event));
        var domainEvent = new TestDomainEvent { EventName = "EntityEvent" };

        // Act
        entity.TestRaiseDomainEvent(domainEvent);

        // Assert
        var events = aggregate.GetUncommittedEvents();
        Assert.Single(events);
        Assert.Contains(domainEvent, events);
    }

    [Fact]
    public void Equals_WithSameId_ShouldBeEqual()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback1 = _ => { };
        Action<IDomainEvent> callback2 = _ => { };
        var entity1 = new TestEntity(sharedId, callback1);
        var entity2 = new TestEntity(sharedId, callback2);

        // Act & Assert
        Assert.Equal(entity1, entity2);
    }

    [Fact]
    public void Equals_WithDifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        var id1 = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var id2 = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback = _ => { };
        var entity1 = new TestEntity(id1, callback);
        var entity2 = new TestEntity(id2, callback);

        // Act & Assert
        Assert.NotEqual(entity1, entity2);
    }

    [Fact]
    public void EqualityOperator_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback1 = _ => { };
        Action<IDomainEvent> callback2 = _ => { };
        var entity1 = new TestEntity(sharedId, callback1);
        var entity2 = new TestEntity(sharedId, callback2);

        // Act & Assert
        Assert.True(entity1 == entity2);
    }

    [Fact]
    public void InequalityOperator_WithDifferentIds_ShouldReturnTrue()
    {
        // Arrange
        var id1 = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var id2 = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback = _ => { };
        var entity1 = new TestEntity(id1, callback);
        var entity2 = new TestEntity(id2, callback);

        // Act & Assert
        Assert.True(entity1 != entity2);
    }

    [Fact]
    public void GetHashCode_WithSameId_ShouldBeSame()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback1 = _ => { };
        Action<IDomainEvent> callback2 = _ => { };
        var entity1 = new TestEntity(sharedId, callback1);
        var entity2 = new TestEntity(sharedId, callback2);

        // Act & Assert
        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void Constructor_WithGuidOverload_ShouldUseGuidAsDefaultIdType()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback = _ => { };

        // Act
        var entity = new TestEntity(id, callback);

        // Assert
        Assert.NotNull(entity);
        Assert.IsType<Guid>(entity.Id.Value);
    }

    [Fact]
    public void Constructor_WithoutIdParameter_ShouldGenerateNewGuidId()
    {
        // Arrange
        Action<IDomainEvent> callback = _ => { };

        // Act
        var entity1 = new TestEntity(callback);
        var entity2 = new TestEntity(callback);

        // Assert
        Assert.NotNull(entity1);
        Assert.NotNull(entity2);
        Assert.NotEqual(entity1.Id.Value, entity2.Id.Value);
        Assert.IsType<Guid>(entity1.Id.Value);
        Assert.IsType<Guid>(entity2.Id.Value);
    }

    [Fact]
    public void Equality_ShouldIgnoreCallback()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        var callCount1 = 0;
        var callCount2 = 0;
        Action<IDomainEvent> callback1 = _ => callCount1++;
        Action<IDomainEvent> callback2 = _ => callCount2++;
        var entity1 = new TestEntity(sharedId, callback1);
        var entity2 = new TestEntity(sharedId, callback2);

        // Raise event on entity1
        entity1.TestRaiseDomainEvent(new TestDomainEvent());

        // Act & Assert
        // Even though callbacks are different and one has been invoked, they should be equal
        Assert.Equal(entity1, entity2);
        Assert.Equal(1, callCount1);
        Assert.Equal(0, callCount2);
    }

    [Fact]
    public void Entity_CanBeUsedInHashBasedCollections()
    {
        // Arrange
        var sharedId = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback = _ => { };
        var entity1 = new TestEntity(sharedId, callback);
        var entity2 = new TestEntity(sharedId, callback);
        var hashSet = new HashSet<TestEntity> { entity1 };

        // Act & Assert
        Assert.Contains(entity2, hashSet);
    }

    [Fact]
    public void RaiseDomainEvent_CallbackException_ShouldPropagate()
    {
        // Arrange
        var id = new EntityId<Guid, TestEntity>(Guid.NewGuid());
        Action<IDomainEvent> callback = _ => throw new InvalidOperationException("Callback error");
        var entity = new TestEntity(id, callback);
        var domainEvent = new TestDomainEvent();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => entity.TestRaiseDomainEvent(domainEvent));
    }
}
