# AI Agent Guidelines for Slo248.Net.Basis

A lightweight Domain-Driven Design library for .NET 10.0+ providing type-safe abstractions for building robust domain models.

## Architecture Overview

**Core Philosophy**: Strongly-typed, event-sourced aggregates with value-based equality for value objects and identity-based equality for entities.

**Key Components** (`Slo248.Net.DDD/Models/` and `Slo248.Net.DDD/Services/`):
- **`BaseEntity<TId, TEntity>`** – Base for all domain entities; equality is based on `Id` only (identity-based)
- **`AggregateRoot<TId, TEntity>` / `AggregateRoot<TEntity>`** – Event-sourcing root with uncommitted event tracking
- **`Entity<TId, TEntity>` / `Entity<TEntity>`** – Child entities that raise events to their parent aggregate via callback
- **`ValueObject`** – Immutable objects with value-based equality (`GetAtomicValues()` for comparison, not identity)
- **`EntityId<TId, TEntity>` / `EntityId<TEntity>`** – Strongly-typed wrapper ensuring compile-time type safety (e.g., `EntityId<Guid, Order>`)
- **`IDomainEventPublisher`** – Abstraction for publishing committed events (integrates with MediatR)
- **`IRepository<TAggregateRoot, TId>`** – Data access abstraction; handles persistence and event publication

**Design Decision**: Dual generic parameters (`TId`, `TEntity`) enable static type checking across the entire domain model, preventing accidental ID mixing between aggregates.

## Critical Patterns

### Event Sourcing Workflow
```csharp
// 1. Raise domain events through aggregate
public void AddItem(OrderItem item) {
    _items.Add(item);
    RaiseDomainEvent(new OrderItemAddedEvent(Id.Value, item.ProductId));
}

// 2. Repository persists, then publishes & clears events
public async Task Update(Order aggregate, CancellationToken ct) {
    _dbContext.Orders.Update(aggregate);
    await _dbContext.SaveChangesAsync(ct);
    
    var events = aggregate.GetUncommittedEvents().ToArray();
    if (events.Length > 0) {
        await _eventPublisher.Publish(events, ct);
        aggregate.MarkAsCommitted(); // Clears internal event list
    }
}
```
**Key Point**: Events are uncommitted until repository publishes; `MarkAsCommitted()` clears them synchronously.

### ValueObject vs Entity Equality
- **ValueObject**: Equality based on all atomic values (`GetAtomicValues()`). Two `Money(100, "USD")` instances are equal.
- **Entity/AggregateRoot**: Equality based on `Id.Value` only. Two entities with same ID but different state are equal.
- **Testing Impact**: Don't compare entities by state; compare by identity or event contents instead.

### Strongly-Typed Identifiers
```csharp
// Prevents compile-time ID confusion
var orderId = new EntityId<Guid, Order>(guid);
var productId = new EntityId<Guid, Product>(guid); // Different type!
// orderId and productId cannot be accidentally swapped

// Implicit conversion to TId
Guid unwrapped = orderId; // Works via operator
```
**Use When**: Creating new entity types; always use `EntityId<TId, TEntity>` constructor in aggregates.

### Entity Event Propagation to Root
```csharp
public abstract class Entity<TId, TEntity> : BaseEntity<TId, TEntity> {
    private readonly Action<IDomainEvent> _raiseToRoot; // Callback
    
    protected override void RaiseDomainEvent(IDomainEvent e) => _raiseToRoot(e);
}

// Child does NOT inherit from AggregateRoot; cannot track events independently
```
**Key**: Child entities must pass events UP to root aggregate via constructor callback.

## Testing Conventions

**Framework**: xUnit (via `PackageReference` in `Directory.Packages.props`)  
**Pattern**: AAA (Arrange-Act-Assert)  
**Mocks Location**: `Slo248.Tests/Mocks/` – contains test doubles like `TestAggregateRoot`, `TestEntity`, `TestDomainEvent`

### Test Mock Pattern
```csharp
// Mocks expose protected methods for testing
public class TestAggregateRoot : AggregateRoot<TestAggregateRoot> {
    public TestAggregateRoot(EntityId<Guid, TestAggregateRoot> id) : base(id) { }
    public TestAggregateRoot() { } // Parameterless for auto Guid
    
    public void TestRaiseDomainEvent(IDomainEvent e) {
        RaiseDomainEvent(e); // Expose for unit tests
    }
}

// Tests in Slo248.Tests/Domain/*Tests.cs
[Fact]
public void RaiseDomainEvent_WithMultipleEvents_ShouldAddAllEvents() {
    var aggregate = new TestAggregateRoot();
    aggregate.TestRaiseDomainEvent(new TestDomainEvent { EventName = "Event1" });
    
    var events = aggregate.GetUncommittedEvents();
    Assert.Single(events);
}
```

**When Extending**: Create test doubles in `Slo248.Tests/Mocks/` that expose protected members; place tests in `Slo248.Tests/Domain/<Feature>Tests.cs`.

## Build & Deployment

**Target**: .NET 10.0 only (set in `Directory.Build.props`)  
**Package Management**: Centralized via `Directory.Packages.props` (Slo248.Net.DDD, MediatR, xUnit versions defined once)  
**NuGet Package**: 
- ID: `Slo248.Net.DDD`
- Current Version: `1.0.1`
- Targets: `net10.0`
- Include: README.md, icon (`slo248-nuget-icon.png`)

**Build Commands**:
```bash
dotnet build Slo248.Net.sln
dotnet test Slo248.Tests/Slo248.Tests.csproj
dotnet pack Slo248.Net.DDD/Slo248.Net.DDD.csproj -c Release
```

## Integration Points

### With MediatR (Optional but Recommended)
```csharp
public class DomainEventPublisher : IDomainEventPublisher {
    public async Task Publish(IDomainEvent[] events, CancellationToken ct) {
        foreach (var e in events) {
            await _mediator.Publish((dynamic)e, ct); // Dynamic dispatch
        }
    }
}
```
**Dynamic Dispatch**: Events are published via `(dynamic)` to invoke typed handlers without reflection knowledge.

### Entity ID Implicit Conversions
- `EntityId<TId, TEntity>` → `TId` (via `implicit operator`)
- `EntityId<Guid, Order>` → `EntityId<Order>` (for Guid-based IDs only)

**Constraint**: Non-Guid IDs cannot convert; throws `InvalidOperationException` if attempted.

## Code Style & Conventions

- **Nullable**: Strictly enabled (`<Nullable>enable/>`); all null references must be explicit `?`
- **Implicit Usings**: Enabled; no need for standard .NET `using` statements
- **Generics Naming**: `TId` (identity type), `TEntity` (entity type); consistency across codebase
- **Protected Methods**: Only `RaiseDomainEvent()`; subclasses override to store or propagate events
- **Immutability**: ValueObjects must be immutable; leverage `{ get; }` properties
- **Licensing**: MIT License header in all source files (`// Copyright (c) Slo248. // Licensed under the MIT License.`)

## Common Tasks for Agents

| Task | Location & Pattern |
|------|-------------------|
| Add new Entity type | Inherit from `Entity<TId, TEntity>`, accept `Action<IDomainEvent> raiseToRoot` in constructor |
| Add new DomainEvent | Create `record` implementing `IDomainEvent` in consuming project, not in library |
| Test new aggregate | Create mock in `Slo248.Tests/Mocks/TestNewAggregate.cs`, test file in `Slo248.Tests/Domain/NewAggregateTests.cs` |
| Add repository | Implement `IRepository<TAggregateRoot, TId>` in consuming project; publish events after `SaveChangesAsync()` |
| Update NuGet dependencies | Edit `Directory.Packages.props` version entries; rebuild with `dotnet build -f net10.0` |

## Quick Reference

- **Equality Debugging**: Entity equality = ID only (check `BaseEntity.Equals()`)
- **Event Loss**: Forgetting `MarkAsCommitted()` in repository causes duplicate event publication
- **Type Safety Violations**: Passing wrong `EntityId<>` type caught at compile time; use type parameters strictly
- **Child Entity Events**: Must use callback (`_raiseToRoot`); direct `RaiseDomainEvent()` calls won't propagate to root
- **NuGet Publishing**: `GeneratePackageOnBuild=false`; call `dotnet pack` explicitly for Release builds

